int MaxProfit(int[] prices)
{
    int n = prices.Length;
    int hold = -prices[0];
    int cash = 0;

    for (int i = 1; i < n; i++)
    {
        hold = Math.Max(-prices[i], hold);
        cash = Math.Max(cash, hold + prices[i]);
    }

    return cash;
}