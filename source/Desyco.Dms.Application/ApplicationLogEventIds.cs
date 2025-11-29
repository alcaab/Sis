using System.Diagnostics.CodeAnalysis;

namespace Desyco.Dms.Application;

[ExcludeFromCodeCoverage]
public static class ApplicationLogEventIds // 1000
{
    public static class Seeding // 1000 - 1099
    {
        public const int SeedingStarted = 1000;
        public const int SeedingCompleted = 1001;
        public const int SeedingExecution = 1002;
        public const int SeedingError = 1003;

        // Entity Seeding
        public const int EnumEntityDeleted = 1010;
        public const int EnumEntityDeleteSkipped = 1011;
        public const int EnumEntityDeleteError = 1012;
    }

    public static class Pipelines // 1100 - 1499
    {
        // Validation Events 1200 - 1299
        public const int NoValidatorsFound = 1200;
        public const int ExecutingValidators = 1201;
        public const int ValidationPassed = 1202;
        public const int ValidationFailed = 1203;

        // Authorization Events 1300 - 1399
        public const int CheckingPermissionsForRequest = 1300;
        public const int NoAuthGuardsFound = 1301;
        public const int AuthorizationSuccess = 1302;

        public const int UserIsNotAuthenticated = 1340;
        public const int UserDoesNotHavePermission = 1341;
        public const int UserDoesNotHavePermissions = 1342;
        public const int CheckingUserAuthentication = 1343;

        // Error Events 1400 - 1499
        public const int UnhandledExceptionWhileProcessingRequest = 1400;
        public const int JsonDeserializationError = 1401;
    }

    public static class Stores // Stores Events 1500 - 1599
    {
        // Store Client Service
        public const int ApiStartingRequest = 1500;
        public const int ApiDeserializedStores = 1501;
        public const int ApiDeserializationFailed = 1502;
        public const int ApiRequestFailed = 1503;
        public const int ApiCallFailed = 1504;
    }

    public static class Jobs // Jobs Events 1600 - 1699
    {
        public const int ImportStoresFailed = 1600;
        public const int StoreImportJobSummary = 1601;
        public const int StoreArticleSalesVariantImportJobSummary = 1602;
        public const int StoreArticleSalesVariantImportJobFailed = 1603;
        public const int ImportArticlesImagesJobSummary = 1604;
        public const int ImportArticlesImagesFailed = 1605;
        public const int ArticleImportJobSummary = 1606;
        public const int BranchArticleSalesVariantsImportJobSummary = 1607;
        public const int BranchImportJobSummary = 1608;
        public const int NoArticlesToHyphenate = 1609;
        public const int ProcessingHyphenation = 1610;
        public const int HyphenationResultDetails = 1611;
        public const int HyphenationError = 1612;
        public const int SupplierImportJobSummary = 1613;
        public const int ArticleImagesNotFound = 1614;
        public const int NoArticleImagesToExport = 1615;
        public const int NoArticleXmlToExport = 1616;
    }

    public static class DataSync // DataSync Events 1700 - 1799
    {
        public const int BulkCopyStarted = 1700;
        public const int BulkCopyNoRecords = 1701;
        public const int BulkCopyProgress = 1702;
        public const int BulkCopyFinished = 1703;
        public const int CleanupSkipped = 1704;
        public const int CleanupStarted = 1705;
        public const int MergeStarted = 1706;
        public const int MergeFinished = 1707;
        public const int TableCreationStarted = 1708;
        public const int TableCreationFinished = 1709;
        public const int TableCreationAborted = 1710;
        public const int SyncBatchNoOperation = 1711;
        public const int SyncBatchStarting = 1712;
        public const int SyncBatchExecutingStrategy = 1713;
        public const int SyncBatchError = 1714;
        public const int SyncBatchCompleted = 1715;
        public const int BulkCopyError = 1716;
        public const int BulkCopyPageProgress = 1717;
        public const int BulkCopyBatchStarted = 1718;
        public const int BulkCopyPageEmptyResult = 1719;
    }

    public static class Article // Article Events 1800 - 1899
    {
        public const int ApiStartingIdsRequest = 1800;
        public const int ApiDeserializedArticles = 1801;
        public const int ApiDeserializationFailed = 1802;
        public const int ApiRequestFailed = 1803;
        public const int ApiCallFailed = 1804;
        public const int ArticleWithMultipleImages = 1805;
        public const int ArticleWithoutImage = 1806;
        public const int ArticleWithImageEmpty = 1807;
        public const int ImportArticleImageIdFailed = 1808;
        public const int ImportArticleImageFileFailed = 1809;
        public const int ApiStartingFileRequest = 1810;
        public const int StartingImageImport = 1811;
        public const int ImageImportFailed = 1812;
        public const int ArticleVisibilityChanged = 1813;
        public const int ArticleNotFound = 1814;
        public const int ArticleExportJobStarting = 1815;
        public const int ArticleExportJobFinished = 1816;
        public const int ArticleExportJobError = 1817;
        public const int ArticleExportProcessingBatchOfStores = 1818;
        public const int ArticleExportFinishedBatchOfStores = 1819;
        public const int ArticleExportErrorProcessingBatchOfStores = 1820;
        public const int MissingArticleImagesNotificationEmailSent = 1821;
        public const int MissingArticleImagesNotificationEmailFailed = 1822;
        public const int MissingArticleImagesJobNoArticlesFound = 1823;
        public const int MissingArticleImagesNotificationNoEmailForRecipient = 1824;
        public const int MissingArticleImagesNotificationArticlesFound = 1825;
        public const int MissingArticleImagesNotificationFailedToRetrieveArticles = 1826;
        public const int MissingArticleImagesNotificationRetrievingArticles = 1827;
        public const int MissingArticleImagesNotificationFailedToCreateTable = 1828;
        public const int MissingArticleImagesNotificationFailedToRetrieveInspectorUsers = 1829;
        public const int MissingArticleImagesNotificationFailedToEnqueueJob = 1830;
        public const int MissingArticleImagesNotificationFoundRecipients = 1831;
        public const int MissingArticleImagesNotificationArticlesWithoutImages = 1832;
        public const int StartExportingXmlFile = 1833;
        public const int NoArticleFoundForXmlExport = 1834;
        public const int XmlFileSuccessfullyExported = 1835;
        public const int ArticleExportStarted = 1836;
        public const int ArticleExportReplacingFile = 1837;
        public const int ArticleExportReplaceFileFailed = 1838;
        public const int ArticleExportCompleted = 1839;
        public const int ArticleExportRecordDeleted = 1840;

    }

    public static class Branches // Branch Events 1900 - 1999
    {
        // Branch Client Service
        public const int ApiBranchStartingApiRequest = 1900;
        public const int ApiBranchDeserializedBranches = 1901;
        public const int ApiBranchDeserializationFailed = 1902;
        public const int ApiBranchRequestFailed = 1903;
        public const int ApiBranchCallFailed = 1904;
    }

    public static class FileSystem // FileSystem Events 2000 - 2099
    {
        public const int ImageWriterWritingFile = 2000;
        public const int ImageWriterFileWritten = 2001;
        public const int ImageWriterDirectoryNotFound = 2002;
    }

    public static class Settings // Settings Events 2100 - 2199
    {
        public const int WriteableDirectory = 2100;
        public const int DeletableDirectory = 2101;
        public const int UnauthorizedAccessOnDirectory = 2102;
        public const int CannotWriteToDirectory = 2103;
    }

    public static class CustomGroups // CustomGroups Events 2100 - 2199
    {
        public const int CreatingCustomGroup = 2100;
        public const int CustomGroupCreated = 2101;
        public const int UpdatingCustomGroup = 2103;
        public const int CustomGroupNotFound = 2104;
        public const int CustomGroupUpdated = 2105;
        public const int DeletingCustomGroup = 2106;
        public const int CustomGroupNotFoundForDeletion = 2107;
        public const int CustomGroupDeleted = 2108;
        public const int GettingAvailableSubGroups = 2109;
        public const int AvailableSubGroupsFound = 2110;
        public const int GettingCustomGroupById = 2111;
        public const int CustomGroupByIdFound = 2112;
        public const int GettingCustomGroupByMainGroupNumber = 2113;
        public const int CustomGroupByMainGroupNumberFound = 2114;
        public const int GettingAvailableCustomGroupArticles = 2115;
        public const int AvailableCustomGroupArticlesFound = 2116;
    }

    public static class Authentication // Authentication Events 2200 - 2299
    {
        public const int LogTaskCanceledException = 2200;
    }

    public static class Mailing // Mailing Events 2300 - 2399
    {
        public const int AttachedFilesToEmail = 2300;
        public const int EmailSent = 2301;
        public const int EmailFailed = 2302;
    }
}
