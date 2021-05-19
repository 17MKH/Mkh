using System;
using System.Collections.Generic;

namespace Utils.Tests
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public IList<string> Tags { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
