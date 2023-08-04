namespace HCEngine.Store
{
    public struct ReadOnlyProduct : IReadOnlyProduct
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public StoreType StoreType { get; set; }

        public ProductState State { get; set; }
        public ProductSettings Settings { get; set; }

        public ReadOnlyProduct(string name, string category, StoreType storeType, ProductSettings settings, ProductState state)
        {
            Name = name;
            Category = category;
            StoreType = storeType;
            Settings = settings;

            State = state;
        }

        public ReadOnlyProduct(IProduct product) : this(product.Name, product.Category, product.StoreType, product.Settings, product.State) { }
    }
}