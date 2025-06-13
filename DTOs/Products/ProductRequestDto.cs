using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Prodify.Models;

namespace Prodify.Dtos
{

    public class ProductPaginatedRequest : PaginatedRequestDto
    {
        public string? search { get; set; }
    }


    public class CreateProductRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        // [Required(ErrorMessage = "Product photo is required")]
        public IFormFile? Photo { get; set; }

        [Required(ErrorMessage = "Display start date is required")]
        public DateTime DisplayStart { get; set; }

        [Required(ErrorMessage = "Display end date is required")]
        public DateTime DisplayEnd { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public ProductStatus Status { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }
    }


    public class UpdateProductRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }


        public IFormFile? Photo { get; set; }

        [Required(ErrorMessage = "Display start date is required")]
        public DateTime DisplayStart { get; set; }

        [Required(ErrorMessage = "Display end date is required")]
        public DateTime DisplayEnd { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public ProductStatus Status { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }
    }
}
