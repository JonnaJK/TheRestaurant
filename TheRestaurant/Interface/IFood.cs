namespace TheRestaurant.Interface
{
    internal interface IFood
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int QualityScore { get; set; }
        public bool IsVegetarian { get; set; }
    }
}
