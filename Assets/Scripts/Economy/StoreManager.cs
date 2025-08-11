using System.Collections.Generic;

public static class StoreManager
{
    public static List<CosmeticItem> Catalog = new();

    public static bool Buy(string id, int price)
    {
        if (!CurrencyManager.Spend(price)) return false;
        InventoryManager.Owned.Add(id);
        InventoryManager.Save();
        CurrencyManager.Save();
        return true;
    }
}
