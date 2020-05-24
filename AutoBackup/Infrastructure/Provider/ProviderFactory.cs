using AutoBackup.Enum;

namespace AutoBackup.Infrastructure.Provider
{
    public class ProviderFactory
    {
        public static IProvider GetProvider(DataProvider dataProvider)
        {
            IProvider provider = null;

            switch(dataProvider)
            {
                case DataProvider.Dropbox:
                    provider = new DropboxProvider();
                    break;
                case DataProvider.GoogleCloudPlatformStorage:
                    provider = new GoogleCloudPlatformProvider();
                    break;
            }

            return provider;
        }
    }
}
