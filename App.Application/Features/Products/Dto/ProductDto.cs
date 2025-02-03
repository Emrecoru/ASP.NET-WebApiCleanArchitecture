namespace App.Application.Features.Products.Dto
{
    public record ProductDto(int Id, string Name, decimal Price, int Stock, int CategoryId);
    /*
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public decimal Price { get; init; }

        public int Stock { get; init; }
    }
    */
}
