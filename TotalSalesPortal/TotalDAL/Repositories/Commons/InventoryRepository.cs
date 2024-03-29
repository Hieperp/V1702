﻿using System;
using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public InventoryRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
            this.totalSalesPortalEntities.Database.CommandTimeout = 300;
        }

        public bool CheckOverStock(DateTime? checkedDate, string warehouseIDList, string commodityIDList)
        {
            List<OverStockItem> overStockItems = this.totalSalesPortalEntities.GetOverStockItems(checkedDate).ToList();
            if (overStockItems.Count == 0)
                return true;
            else
            {
                string errorMessage = "Tính đến ngày: " + overStockItems[0].DateOverStock.ToShortDateString() + ", những mặt hàng sau không còn đủ số lượng tồn kho: " + "\r\n" + "\r\n";
                foreach (OverStockItem overStockItem in overStockItems)
                    errorMessage = errorMessage + "\t -----" + overStockItem.Description + "\t" + overStockItem.DescriptionOfficial + "\t" + "[" + overStockItem.Quantity.ToString("N0") + "]\t" + "\r\n";

                throw new Exception(errorMessage);
            }
        }

    }
}
