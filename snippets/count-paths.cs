CountPaths(4, 4);

int CountPaths(int m, int n)
{
    int[,] dp = new int[m, n];

    for (int i = 0; i < m; i++)
    {
        dp[i, 0] = 1;
    }

    for (int j = 0; j < n; j++)
    {
        dp[0, j] = 1;
    }

    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {
            System.Console.WriteLine($"Calculating dp[{i},{j}] = {dp[i, j]}");
        }
    }

    return 0;
}