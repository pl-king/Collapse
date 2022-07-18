﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml;
using System.Text;
using Newtonsoft.Json;

#if NETCOREAPP
using Hi3Helper.Data;
using Hi3Helper.Shared.ClassStruct;
using Hi3Helper.EncTool;
#endif

namespace G_Game_Dispatcher_Test
{
    internal class Program
    {
        const string Key = "caffbdd6d7460dff";
        const string URL = "https://{0}.yuanshen.com/query_cur_region?version={1}&platform=3&binary=1&time=256&channel_id=1&sub_channel_id=1&account_type=1&dispatchSeed={2}&key_id=3";
        const string MasterKey = "D4F8D4A6F6CF9AD4E78FF4E1D696CA88D9DF8FF7F5DDF3DC87E1D08A8598A9D1DED7D4DD81C2EC938FECB0E8F493B2F4B2DBD39FC99CA2FBCCD3DBE190E2F494EEDEBAE7F5A0C7F1BDE5EAA3FFD888B2E89BD1D0B198F5D0D2E8D7D8C5DAF5C080E2FD8AFC9CD5CFE3D6B7F383EEB3D0E79985C1C3A1E7C8AED2EAA2EBCEB6C1F0AEB0D6A1ECE3B6C5FBA0C0B1A0CCE5AEFDDFAFD2FC85C0EEADE2C0DE9AC893E5EED4B3F0A5F4F7AAE6EE88DC9CABC6CE81CAEFD0E6E380EACEB5B3B6ACD6E5AD97BBC8F0C587F7EA96F2BAD4EFFF97D2C486ECF2DDC4D5D597BBC8F8D293EDE886EFF0D696D7D98C9CA4EBF191E6C7AFDCE4BEDFCDB0D8C9A1CBD6909EC0DED6C38CC5E186F9C8BBD3D0D18FC3A8B6C396D5B4B0DFBE9FC4CC8FC0C282C2B091C5E8A1D6D291E4EA92C4C5A1C3C281E9DBADC5EABAE4CE83D9ACD2CCDDDEBFBACCD1BAD4FBB99DDBFBBAB6E2B4E8ABACC6E8D485D3D5FAF0BBB5F4BDEFC68D8CC281E7C08CAED581D3B0AF8D9AB3B2BEDAD6B4A3E9B1AAD9C3C8C5DFD7D4F1BAEDCD93DAF2D4D7DC85C6D58CE2D58D85C7A1E8EF89C7FD80FFEBD3CA97DEBEA9B2BFB8ACFAB984D4C28FCDE389F8C083CBD283D3D88DF6C3BBF7C0D8F9F1978ADC8FD7BF87FBCED0C3F2A9C781ACEDCC87D4C983E1CF8EECDEDBFBF092B9C89E99C0938DDAD1DAF5D1B1FC9BF3BE90F9C2B0EBB4BBE7E5D1CEEEB4CA97DEBEA9A7D1BAD4EED6D9FAEE91D4F1A7E5C5BFCFEE8E8AF980EEC5A5EAF7BC9CB6ABE5E98AC6C9D2D9F4BDD2D5B4ED8191FBFCB9AEF7AFE3F38FF0DD93F5F2D4DBE791C4F6AECFEBA0B2CCD4E4C59EE2CAADD0E2AAD2D686E0F0DA9EE8A2D9FBDEBFBACCC5D5D696CE89CBCF91F1E3B2BFD4DC85F180CC9BBBC4B1D4CFBCA385C89FE9C880E8D1A5C2C7D9CEAC918EFDD6FAD1B2CBE685FFB4ACDBFCA5B0F1ACD2F0B2CFCEABE998BBB3B490B6C5A0EEAC92E8CCADBBC9CCF9E0ADE8CE82CCC9B0CED09BD6EE9F97BADB92E38DF4E391F2E1B994BBA383C5D3D5F0B9C2FC9EE7E0B0D8CB97F0FF81C3F2A1DCEBB0EC9ABBB4C5AFC7DDB1CFB5928998A4DAF6AACDEF8ADAB4B7FBDF99CAF08DEAF19FC3F7C8C4EDD0B7D48FEDD7DACFE1A0D7CCABC7B68BE6C5DBC9E69DCFE484CFB084C3C08BECE6D0D4D08DF4EFAAE8D5BCD3D49FFBDF99FBB58ED9F482F9D6A1DCED84E6F08EF4D1B9E6E08088DB82EAE587C6E3BC9EB2D2FBD08CB7C1B0F4AFA1D3D397F98187CCE9A8F8B3D1E4CBB3DFDC89C7BBDFAEC0D696A8B5EEEBA8E7FFB5E0E89DCFB9";
        
        const string pubRSAKey = "<RSAKeyValue><Modulus>yYlF2xKeHRDZUSsXlLoSk/YAb2oIOwpbUeO4I+5GfWoybSpde4UnOlZgpKIDgltF3e9ST8bqIwuBxMoJTpOAnEKLNuBDdSeefHwhFqdczgeETxySwFKScmti1QRwgacrlgWglmaYCaeQrqbBceF9JbF4npi6S3+eFpw0j4rPjlE3vjh1AopaZQWAHGZI8Ixr7LDebe/uF8i7OCWXpkPKUTJnCEpyqM5H+pLN3MWRiL7mBR4XFqwKQr8J27Y3LN1iX9927hMsvAnh9PWoHzqpDTqIBF7w1ifYs3XQ3EMbf0zqc26UZXUaI5pD6qXNm3STz94SrfYqYY1R3Npz/Syaww==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        const string privRSAKey = "<RSAKeyValue><Modulus>02M1I1V/YvxANOvLFX8R7D8At40IlT7HDWpAW3t+tAgQ7sqjCeYOxiXqOaaw2kJhM3HT5nZll48UmykVq45Q05J57nhdSsGXLJshtLcTg9liMEoW61BjVZi9EPPRSnE05tBJc57iqZw+aEcaSU0awfzBc8IkRd6+pJ5iIgEVfuTluanizhHWvRli3EkAF4VNhaTfP3EkYfr4NE899aUeScbbdLFI6u1XQudlJCPTxaISx5ZcwM+nP3v242ABcjgUcfCbz0AY547WazK4bWP3qicyxo4MoLOoe9WBq6EuG4CuZQrzKnq8ltSxud/6chdg8Mqp/IasEQ2TpvY78tEXDQ==</Modulus><Exponent>AQAB</Exponent><P>9ci4i5gUVSGo3PkIpTTXiNm7pCXTPlkDTxXzhxMlx8sgrh7auoLwMVOV0DQW1V84a3RXTwf/HalEKEY69TAYbmef0OqqHoGMHJStbjPaGdfNPdm5IOHp5qmIIHWOX2Z4nSyeEXY+z+GpYYvZvdKQIJ73SpVPM5U54s7phQIg6r0=</P><Q>3Cx9CQCr/THDyd5EY1OudeKa9tL5Vc8gXfzCJ2WO5s03sNjlwgVNAmudMFYpu7P+31onxBfpMUvRyL/2+E8mhOF8vXa8vaRYZiBaRZE+apoFbfLPsezmu37G4ff/sDnDm+aQSDU1kmCewnSsxRO7VDo8zkIGDo6nIdjhOEFvypE=</Q><DP>ML8ciuMgtTm1yg3CPzHZxZSZeJbf7K+uzlKmOBX+GkAZPS91ZiRuCvpu7hpGpQ77m6Q5ZL1LRdC6adpz+wkM72ix87d3AhHjfg+mzgKOsS1x0WCLLRBhWZQqIXXvRNCH/3RH7WKsVoKFG4mnJ9TJLQ8aMLqoOKzSDD/JZM3lRWk=</DP><DQ>PIZ+WNs2bIQhrnzLkAKRExcYQoH8yPoHi87QEMR6ZDhF9vepMY0DfobWz1LgZhk1F3FRPTbVhBezs9wRqHEZxa22/N6HRBrJsklyh21GG0f79h2put/FDgXr5nKmd2tpupHHWBJIh9THz+0DEao69QyNaqX7xESy7TsRrsVOVgE=</DQ><InverseQ>mlWr8mOkpY92UUO4ipPXx5IHv2xZfs4QDcUX1lTmDAvJg9oBw7KvQiHQqdTINLSaVi2hoMgzNZIAoWWLH3+I0cRwuHM7wLaD0pcVlxdpy99aid75Nmc83GuBkhwCJ6HVwayrLWr+UiCqLFik9mMrMYB5QPUptn+J9PRoxW7JRB4=</InverseQ><D>uLj7GJOALEnu+dALug8+5EnyIHQ4SeOAIrL05ny2rjBWS7X8X4wQ4QsE8bg+15wmQMR5ve08vgKkqSpv62kELL7VmpTIQamGp84w2DEb9p4idbxo5t1q0MQWhBfsjrb62bCuX0E7JaiJyKpJyEB+34I2sye2dvA9fLGDY9+6nxVkkspoBaPkqEvwShK9tNJaUQP6Ghl4h3MiDoyYnT+m+1BnrO7oTF1Ly636M5grEqrJcVzuVJOVzf31peC8Qhl+5qTXz2SE+WAUox5YhZDZcSI8iYPDkSxovNjNnLssad/a/dxermgoy7W/q3cJRrq+56YF1JCn1kCX/VhO7mq+gQ==</D></RSAKeyValue>";
        const string Content = "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";

#if NETCOREAPP
        static async Task Main(string[] args)
        {
            string GenshinKey = "BEEEF1B2C4DE85F3C389E2C5A6E5E6B1F19D92CBEDD6B3D6AAC2A888DBE497D5D5BAD5CF8E9FEECCDEDDACD6F1DBF6D1BFC5CE92F0D38FC4E3B3B2D598EECAD188FFA8D2E2D6C6DE8ACCCEBFF6F992DBAD94F8AFB1D8BFACEFF3D0E9F591DBC6A9FAD4D3E4DEC8E7C99AF7C59C9FD4D1FE9E93ECF692E5DCDFC7B28EE5E5D6CFD5A8E7C19EE1D6A8D0C2CCC1E59BC7C5D9D0FE91EED2CCD2D780C9C69AFBEFB6CCF2D1B3C1A7CACF8EF0C4AFEBD285B5DC97B7F4B2E5CB85CD92B1B6EB9BF9D08499D48ED7C28CC5C0A5E3EF98E4DF95F2E882F6CCD6D8CEBBD2F6D59699D6D5BF8CC6F7C3E8ECD3DB93BBB5D09BDBE7DBFAB2BEF0EE84C3AD99D9B182CCE584E5D8A9BBC5B7F0C79BECD29ECFFCAFDAF4D5D3E5BA99C18FCEF8B1B6F784D0E3BCF0E592CAD9D2E5CBD1E5EC82FCF4DEE79EA1D2D5D0AEAFAAE8E3D6FEFCB4B4BEA4B4EFA7E3C6DFD3C784E0EA94CCE58DC9F08BF1E2B7F8C290B6B2819EF682F1C7A8D5B186E2D7DAEBC4D1E5F8CCC9D1ACB0B3AAD8B5A3FBDAB0E7CFB5F0B381C9D283F3E1D7F8D5A6F1E0A99BB089CAFA81C9C987B4E889DFDE88D2D287C8F7B6B0D5BAEDEDD3D9EE97C5B4A9F6B4DEF0CA9385C2A5CAE184F7C181C7EA9FE7CE8DEBFC92EBE286E8EC85F098AAC1E180D4F28BC4C29DC998A7D5EF85D7E0ACECEC83F5D9AAECB586F6DEB1C0C9D5F0D0C8B6CECCF0E58BD8FD80D89ABBCDCF80D1CEAFEEE284EDFCA1EEC3A5E7FC8EEFF4B5C89388C5E089B8F2A281B196849B85E0EC8FE3F29FE0EF88FFD88DD7CCACEFB68C9DF5B5FBDEB7DAB28BD8D2B9D8BEAC89EDD2C8CEB7D2D0A4D3DD898FED94ECD082D7E7839ABF828CC5B0DBC7ABB9C6C39AF4D7D9DEA1DAC1D7D4D2DAF0D3D1FAFFCCCFB388C7F0A1F8C994D6C8D4B6F1D3FBD7B0FFF5A58ECCD5F0E8D0FBCCD8C6EE86CADCD5B7B792C9CAA1C4B6ACC7FE97B4EEB5C0CFB9CBCDACD2F088F4CCBAE8B6DA93FD9FF3E2B6ECC4D4B8B385FBD0AECCC485CCD597B6FEBF85E9A38EEFA2F3EE89D3EFC7DCEABED293D3C0CAAEF9F6DFE8E497C4EFD0E7C1B2E4FDAFDCD78ED4FAC8B4CA93B3F2809EC0939681B9E8EB8ECDDDAAEBD4D6D5E282B4D6A9D1D6D1FFC393F19D96C9C993CFF3BDE0CFD4D2FD88CDD790E4E08AE3CDAEF19BC8F7D1A6B5DEA5DFCAD1DAC78BB5FEB7CBF0869AB4BDDE9E9BB1C9AFF9D08C99B4A2E9FE9BECB78AB2C9B09ABEB6E5C9B4E3CAB6C6F6B9C8DDD0DA9C8CF1CE9BEAC88DF9E497F6ECA1D8C58EF2E38CFCD4D085EF84EECE97D1ABD9E2DFB7F7E6A0C3F38DD3D6DAE1D6D7CAC185B1DEABECD08D9EC3D6E7939BD5D684C0E7A79AF195DF9B96B1EDA4C2CD82FDF78DE5C981D2B6A2F2C0DFE6E2A9FF988DADB5ADC2CFDB9FED82F1F990F5F48ACDDD8292BEA3ED9BB2DBCBB9E4FC859FE4BD8BF293A9EC81CFD69ED9E48FDCCBABD6B491B6F58DFFDDBFD193D0F8E295C7CB999DFED2EFC6B3E9DEB1FBB5AB99A8D6DFED92C4E98AAACF8AEBE3D3D9F9D6EBB291DBCBBB85C5D4ECFFB9C9F4B1DBE19FEED0A3C5EBD2B3DFA9C7D0AD92CDD0DC9899CFE897AEEFDB9AB1AADFE78FD2E289F4B783EBD4A4C4D390D6C9B6B3E6ABE9F184C9E8A9D4BE8ACACE82C1E9CCF6D3D7E4CFB9B9FEA9FBE48D8CD09ABAE396EEE1BDC3BFD084FCADA9C5D7D3C0C3E1ED8BD5FE8BD7D382B6DCB198E28E899E8AF0DC87C9F19CEDBE8EFCE5A9F7F687B1CBDBDFEAD78CC9B5F1DF9BB4B68FFFEE9388EDADC9C38EEFABDEEBEA8992EC96F4DC8CEDBD8FC4F285D0818CDBF29BC0CF8FC4F09784C0B5BBE1A8ECCF9BE9B4978AE2B0E4E0BBC7C7A3C5DED1EC9CD6C6D088B7D787C6F2AF8AD080E8D6D1D6EF9BFFE5AAF7999BE9F59ACCCBA5DAACB2DF9D90C0CBD7DBC791F0B2C8E8C98FC1D293F7FC90FEEBAC8492A8CAFFABEFC5AC92FEACEE85B2D1C893C0D0DDF2D08BCA98DAB4B3C8E4B5D8F9FDD3EACBACCBCEDBAECD9CD2EF93D1E9D2D4D488F7B799FEB6B2D7F9A1E1D3ADD7D78ADCB39DDE9AD7F4F2D3B2C3BB9CD486D0C388D8E487C6D4AAC1ECB184DBBAE3D6B4E0D5A1F3ACDEC59C92B5DE86C3C9AECBD48F8EC192E3CA8ED7E5ABEBE094E4CB81B7DCB5B2B7B1E4C288FCEC87E1E886C2B5A6E7DE908BD887C8E8D3D6C8A1F0F289EBED93C5CB86D8F787EDE182ECDDACA9DCD1B5B786DFCFD38DC6DBC5B2A4F8D3ADE5FEAFCCFAB6D2AD8ACDFC99CFF58FEFD0D7D6CFB5B7C1BCC0A88BD7C388EFF1D0CDAB9CEFC9A5FAE1AFE6C48DD3ECB0EFF0AFFEFEB5CCDE97E6DDAFCDF08BEFEEA7D4B391D9C99899D2BEFEC58EF6FED7D4DC91E6EA94F1D0B9E6E9D5AEE981C7F1ACF1D8B4F3B289EAB2B2FDE3BDCCD3AEE3B09AE4B5DBCED2B0D09BBAC6F2D3D2CED0D0F4A888F08EF2D0B1B6C18BC5D5A8F99B97C9C8D1EAE3A9C6E9D4859CA7E5D0ADAED58FE9D796D8D9B0B7D4A2AEC5AB9AE9A5CADBDAE6D587D2DEADE9B097E9DFB7F0B1B0B5F1AD85CC918CC9A2CED294C6E292C2E3A3FCE6A4DAB2D7AEE9BDE5B2A1DED28BC8D0B3C6CEDBE8EFA8D19AA8FBDE9AF4CB9898F0D1D8CE94E7E8B0F3E6D09BC68BD6E797D3B3A5E2F6BBCFCDBEFBF991B1D2D4DBEA8098F0AAEFC291EAC0B3CAB783DEF096D3C382C8C9A0F2DDA6CDC3AA8FEBA1CBF0ABE3C89CE9FD9DF4D0A5E6F0B2B3EBB1FBD2B4CBC5A7F3B3B6AAE6D1CBE8B5F2DAD2E6BF94ECDE85F9C2B6D4C6BBF0E9B0EEC98E93E5A9DF8196FAEEB1CAF1B298E09FD9C38DEDD68FF3CFDDE0CD9FEF9CCCEDD7B0AEB2B0E2D1D3CCFDB6F5C08AF4F698EDC6AF8B98A7E5E2CCDBB39BE1D3D188FBADE4ED8FD8CABF9FD79FDFC4C8D3C8AEC0DD839BB488DBC1D0CAF4A4B4C6A499E18D89FD92F5C2D1D9C1A0C5F59085ECCCE4C892CAC5A4E1B7A9D1DA8CD5B79BC7F1DF9AD1B296C0D6F3E1A7D8FC849CF7A0C5D390C8B4D0EEBC8FDECA90CD929BD6F4AEE8D5ABEFFED5F4C68CBBD7B6C6F2A0D0F6D0CDE194BAFCBBC5F6B99CB78B849F9BF2F49BC8C1DCCCD592E99CD6C5EC96E7B5AEF8EBA9D9EED1D7D6D2C0F3A593ECA0DAFDB7B5F0A1CDF0B9E4DD9DCBE08DB5E4B1CBBDB9E3E5A0F7C99BB0D193F2EF9FE3C084D781ADD3C792D5CBDDE3F6B592C097E7C4B5C2CA84D9A88BEE9985EDD0BBE4EEDFDDEBACFA81BBC6E7B4C5C5BFC0E5D1FAC39BDAD0BBF1CFBAE1CEAF8AF396E5C2A2D6F3DCCBCE93D993D2C0DEB2AAE6C3E5E9B48DCF8FBBB0AFE0CFA3CBE3C8FADB8FE6E7A9D5F59A9EB5D0C49B9AB0FCA4B1F4C3DCE4ACEFE085CAC8B0CAFCA0E7ECD5CBE5CCC5EBB5C5D5BFC3C185C9CEADADDE85C8D087DCDDB7F6C1D1F7CA93FBCC9F85C281DFC2D6E0CB9AC4B5AFC3C486DA9FB1E8F384C3EB83E9FDB2929C8EC0ADD6F2C182C4B68FEAD081E9B6B7C5C78CEFCEBFFFF2D1E4E0B5C7DCDAE9B7A3F1EDC8DBD78CCEF491DBEAB5CE9BB4F0F486CDF08EF3B686CAD091F1DCAEE5C186CDCF9E89CF87E5C4B2F3CB81DDA8A6F1F392ECFEB4CCC6B1DAE8AEDEEEABCAEEB0CBB381CEFEB2C7CBA7C9D487F6FC8DD0FFACF1F994FAFCBAECEDB998C886C5C5BBBBC9D5D0B3A081D3CCF7E1B0CCC996B0ABBAC4F1B4E4C68CB5CE88E8D48EDBCCAB";
            
            GenshinDispatchHelper DispatcherHelper = new GenshinDispatchHelper(0, Key, URL, "2.8.0", default);
            YSDispatchDec Decryptor = new YSDispatchDec();
            Decryptor.InitMasterKey(MasterKey, 128, RSAEncryptionPadding.Pkcs1);
            Decryptor.DecryptStringWithMasterKey(ref GenshinKey);
            Decryptor.InitYSDecoder(GenshinKey);

            Decryptor.InitRSA();
            YSDispatchInfo DispatcherData = await DispatcherHelper.LoadDispatchInfo();
            byte[] ProtoDecrypted = Decryptor.DecryptYSDispatch(DispatcherData.content);
            await DispatcherHelper.LoadDispatch(ProtoDecrypted);

            QueryProperty Property = DispatcherHelper.GetResult();

        }
#else
        static void Main(string[] args)
        {
        }
#endif
    }

    public class GGameDispatcher
    {
        const string SignSalt = "RizMT3Kl+Gv7Fpx7KFWr5HmJ29q+kWy3/cVwSrnRykiw0wdXhZM3Yjqb1GLGH3sK4k06CKjiF+JNbe2BoISSsQ/iy5QhHljMSG3N8WTODKlAZ7Wm7R1CEUHkZxNLg2RZ6QuJS3/rJRZjBIx18Tjwbrbc+sCAAd42LezvPS8Zw+HHzOabHnk1YJ9CRU1El2nizYGs3vQN2WNYlahX1wYxBIJKXPyFn2Nv2F67d0J3gUegczGatdzUE+Z7IvoO54dl7C9YAZOXk6YtKrHnY6UtWuUIcc6giOvRiK6UBBECEVm9lWQ6sRBic09Mbp5FpYjevWOXyFBVDBuFpcDr5eGoaw==";
        const string OldProto = "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";

        private protected RSAEncryptionPadding Padding;
        private protected int EncBitlength;
        private protected RSA _ooh;

        public byte[] DecryptContent(string ContentBase64)
        {
            byte[] EncContent = Convert.FromBase64String(ContentBase64);
            MemoryStream DecContent = new MemoryStream();

            int j = 0;

            while (j < EncContent.Length)
            {
                byte[] chunk = new byte[this.EncBitlength];
                Array.Copy(EncContent, j, chunk, 0, this.EncBitlength);
                byte[] chunkDec = this._ooh.Decrypt(chunk, this.Padding);
                DecContent.Write(chunkDec, 0, chunkDec.Length);
                j += this.EncBitlength;
            }

            return DecContent.ToArray();
        }
    }
}