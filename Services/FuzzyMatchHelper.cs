namespace SearchEngine.Services
{
    public static class FuzzyMatchHelper
    {
        public static int CalculateScoreBasedOnService(string providedServicedeName, string availableServicedeName)
        {
            if (string.IsNullOrEmpty(providedServicedeName)) return availableServicedeName?.Length ?? 0;
            if (string.IsNullOrEmpty(availableServicedeName)) return providedServicedeName.Length;

            int[,] d = new int[providedServicedeName.Length + 1, availableServicedeName.Length + 1];

            for (int i = 0; i <= providedServicedeName.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= availableServicedeName.Length; j++) d[0, j] = j;

            for (int i = 1; i <= providedServicedeName.Length; i++)
            {
                for (int j = 1; j <= availableServicedeName.Length; j++)
                {
                    int similarityinSearch = (providedServicedeName[i - 1] == availableServicedeName[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + similarityinSearch
                    );
                }
            }

            return d[providedServicedeName.Length, availableServicedeName.Length];
        }        

        public static int CalculateScore(string providedServicedeName, string availableServicedeName)
        {
            int maxLen = Math.Max(providedServicedeName.Length, availableServicedeName.Length);
            if (maxLen == 0) return 100;

            int calculatedScore = CalculateScoreBasedOnService(providedServicedeName.ToLower(), availableServicedeName.ToLower());

            // Convert to a 0-100 score where lower distance = higher score
            return Math.Max(0, 100 - (calculatedScore * 100 / maxLen));
        }
    }
}
