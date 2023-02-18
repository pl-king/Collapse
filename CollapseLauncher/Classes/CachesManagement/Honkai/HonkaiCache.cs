﻿using CollapseLauncher.Interfaces;
using Hi3Helper.Data;
using Hi3Helper.Preset;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static Hi3Helper.Locale;
using static Hi3Helper.Shared.Region.LauncherConfig;

namespace CollapseLauncher
{
    internal partial class HonkaiCache : ProgressBase<CacheAssetType, CacheAsset>, ICache
    {
        #region Properties
        private string _cacheRegionalCheckName = "sprite";
        private string _gameLang { get; set; }
        private byte[] _gameSalt;
        private List<CacheAsset> _updateAssetIndex { get; set; }
        #endregion

        public HonkaiCache(UIElement parentUI, string gameVersion, PresetConfigV2 presetConfigV2, byte thread, byte downloadThread)
            : base(
                  parentUI,
                  gameVersion,
                  Path.Combine(GameAppDataFolder, presetConfigV2.InternalGameNameInConfig),
                  null,
                  presetConfigV2,
                  thread,
                  downloadThread)
        {
            _gameLang = presetConfigV2.GetGameLanguage() ?? "en";
        }

        ~HonkaiCache() => Dispose();

        public async Task<bool> StartCheckRoutine()
        {
            return await TryRunExamineThrow(CheckRoutine());
        }

        private async Task<bool> CheckRoutine()
        {
            // Initialize _updateAssetIndex
            _updateAssetIndex = new List<CacheAsset>();

            // Reset status and progress
            ResetStatusAndProgress();

            // Step 1: Fetch asset indexes
            _assetIndex = await Fetch();

            // Step 2: Start assets checking
            _updateAssetIndex = await Check(_assetIndex);

            // Step 3: Summarize and returns true if the assetIndex count != 0 indicates caches needs to be update.
            //         either way, returns false.
            return SummarizeStatusAndProgress(
                _updateAssetIndex,
                string.Format(Lang._CachesPage.CachesStatusNeedUpdate, _progressTotalCountFound, ConverterTool.SummarizeSizeSimple(_progressTotalSizeFound)),
                Lang._CachesPage.CachesStatusUpToDate);
        }

        public async Task StartUpdateRoutine(bool showInteractivePrompt = false)
        {
            if (_updateAssetIndex.Count == 0) throw new InvalidOperationException("There's no cache file need to be update! You can't do the update process!");

            _ = await TryRunExamineThrow(UpdateRoutine());
        }

        private async Task<bool> UpdateRoutine()
        {
            // Assign update task
            Task<bool> updateTask = Update(_updateAssetIndex, _assetIndex);

            // Run update process
            bool updateTaskSuccess = await TryRunExamineThrow(updateTask);

            // Reset status and progress
            ResetStatusAndProgress();

            // Set as completed
            _status.IsCompleted = true;
            _status.IsCanceled = false;
            _status.ActivityStatus = Lang._CachesPage.CachesStatusUpToDate;

            // Update status and progress
            UpdateAll();

            // Clean up _updateAssetIndex
            _updateAssetIndex.Clear();

            return updateTaskSuccess;
        }

        public void CancelRoutine()
        {
            _token.Cancel();
        }

        public void Dispose()
        {

        }
    }
}