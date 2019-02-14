using System;
using System.Collections.Generic;

namespace Global
{
	public class MarketProduct
	{
		public string ProductId;
		public string ProductName;
		public float ProductPrice;
		public string ProductPriceWithLabel;

		public MarketProduct (string productId, string productName, float productPrice, string productPriceWithLabel)
		{
			ProductId = productId;
			ProductName = productName;
			ProductPrice = productPrice;
			ProductPriceWithLabel = productPriceWithLabel;
		}
	}
}
