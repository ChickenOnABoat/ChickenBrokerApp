namespace ChickenBroker.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
    public static class PropertyAgency
    {
        private const string Base = $"{ApiBase}/property_agency";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        
    }

    public static class User
    {
        private const string Base = $"{ApiBase}/user";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }

    public static class PropertySold
    {
        private const string Base = $"{ApiBase}/property_sold";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Address
    {
        private const string Base = $"{ApiBase}/address";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }

    public static class PropertyType
    {
        private const string Base = $"{ApiBase}/property_type";
        public const string GetAll = Base;
    }
}