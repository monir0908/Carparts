namespace CarParts.Common
{
    public class _EnumObjects
    {
        public enum Status
        {
            Stage1 = 1,
            Stage2 = 2,
            Stage3 = 3
        }

        public enum QueryFlagStatus
        {
            Customer,
            Admin
        }

        public enum QueryType
        {
            Text,
            File
        }

        public enum PaymentSettings
        {
            Parcentage,
            Amount
        }

        public enum ImageSizeUnit
        {
            KB,
            MB
        }
        public enum PaymentMethodType
        {
            Digital,
            Cash
        }
        public enum AdminRole
        {
            Super_Admin,
            Admin
        }

        public enum PriceLogStatus
        {
            Increase,
            Decrease,
            Constant
        }

        public enum StockLogStatus
        {
            Increase,
            Decrease
        }

        public enum ShowOnReportStatus
        {
            Show,
            Hide
        }
    }
}
