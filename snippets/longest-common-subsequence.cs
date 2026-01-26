int LongestCommonSubsequence(string s1, string s2)
{
    int m = s1.Length;
    int n = s2.Length;

    int[,] db = new int[m + 1, n + 1];

    for (int i = 1; i <= m; i++)
    {
        for (int j = 1; j <= n; j++)
        {
            if (s1[i - 1] == s2[j - 1])
            {
                db[i, j] = db[i - 1, j - 1] + 1;
            }
            else
            {
                db[i, j] = Math.Max(db[i - 1, j], db[i, j - 1]);
            }
        }
    }

    return db[m, n];

}