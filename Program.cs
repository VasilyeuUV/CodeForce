using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CodeForce
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunContestF();
            //RunContestE();
            //RunContestD();
            //RunContestC();
            //RunContestB();
            //RunTaskH();
            //RunTaskG();
            //RunTaskF();
            //RunTaskE();
            //RunTaskD();
            //RunTaskJ();
            //RunTaskC();
            //RunTaskB();
            //RunTaskA();
        }


        /// <summary>
        /// Read int value, which will be use as words array size
        /// </summary>
        /// <param name="dICTIONARY_SIZE_MIN"></param>
        /// <param name="dICTIONARY_SIZE_MAX"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int ReadInt(int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            if (!int.TryParse(Console.ReadLine(), out var intValue))
                throw new ArgumentException($"Value {intValue} not digit");
            if (intValue < minValue
                || intValue > maxValue
                )
                throw new ArgumentOutOfRangeException(
                    $"Value {intValue} out of range from {minValue} to {maxValue}"
                    );
            return intValue;
        }


        /// <summary>
        /// Read string and convert to integer
        /// </summary>
        /// <param name = "minValue" ></ param >
        /// < param name="maxValue"></param>
        /// <returns></returns>
        /// <exception cref = "ArgumentOutOfRangeException" ></ exception >
        private static int[] ReadIntArray(int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            var arrInt = Console.ReadLine()
                ?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    if (!int.TryParse(s, out var n))
                        throw new ArgumentException($"Value {n} not digit");
                    if (n < minValue
                        || n > maxValue
                        )
                        throw new ArgumentOutOfRangeException($"Value {n} out of range from {minValue} to {maxValue}");
                    return n;
                })
                .ToArray();
            if (arrInt == null)
                throw new ArgumentNullException($"Failed to get data from string");
            return arrInt;
        }


        /// <summary>
        /// Display result
        /// </summary>
        /// <param name="employeReports"></param>
        private static void DisplayResult(int[] employeReports = null)
        {


            List<string> resultLst = new List<string>();
            //for (int i = 0; i < wordRequests.Length; i++)
            //    resultLst.Add(uniqueWordRequestRhymes[wordRequests[i]]);
            Console.WriteLine(string.Join(Environment.NewLine, resultLst));
        }

        //private static void DisplaySortedTable2(int[] clickedColumnNumbers, List<int[]> rows)
        //{
        //    for (int i = 0; i < clickedColumnNumbers.Length; i++)
        //        rows = rows.OrderBy(r => r[clickedColumnNumbers[i] - 1]).ToList();
        //    Console.WriteLine();
        //    Console.WriteLine(string.Join(Environment.NewLine, rows.Select(r => string.Join(' ', r))));
        //    //Console.WriteLine(string.Join(Environment.NewLine, mostPotentialFriends.Select(f => string.Join(' ', f.Value))));

        //     groups1.Any(i => i.Value.Any(around.Contains))
        //}



        /// <summary>
        /// Read Words
        /// </summary>
        /// <param name="dICTIONARY_WORD_LENGTH_MIN"></param>
        /// <param name="dICTIONARY_WORD_LENGTH_MAX"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static string ReadWord2(int minLength, int maxLength)
        {
            string pattern = @"^[a-z]+$";

            var word = Console.ReadLine();
            if (string.IsNullOrEmpty(word)
                || word.Length < minLength
                || word.Length > maxLength
                || !Regex.IsMatch(word, pattern)
                )
                throw new ArgumentException("The word does not match the format");

            return word;
        }




        //###########################################################################################################################
        #region CONTEST_F

        [Serializable]
        private class ProductDTO {

            [JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("parent")]
            public long? Parent { get; set; }
        }


        [Serializable]
        private class ProductModel
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("next")]
            public List<ProductModel> Next { get; set; }
            [JsonIgnore]
            public long? Parent { get; set; }
        }




        private static void RunContestF()
        {
            int testCaseCount = ReadInt(1, 100);
            for (int i = 0; i < testCaseCount; i++)
                RunCONTEST_F(); 
        }

        private static void RunCONTEST_F()
        {
            var strinNumber = ReadInt(1, 1000);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strinNumber; i++)
                sb.Append(Console.ReadLine());
            string json = sb.ToString();

            var dto = JsonSerializer.Deserialize<List<ProductDTO>>(json)
                ?.OrderBy(x => x.Id).ThenBy(x => x.Parent);


            List<ProductModel> models = new List<ProductModel>();
            foreach (var item in dto)
            {
                var model = new ProductModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Next = new List<ProductModel>(),
                    Parent = item.Parent
                };
                var parent = FindParent(models, model);
                if (parent == null)
                    models.Add(model);
                else
                    parent.Next.Add(model);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            json = JsonSerializer.Serialize<List<ProductModel>>(models, options);
            Console.WriteLine(json);
        }


        private static ProductModel? FindParent(List<ProductModel> models, ProductModel model)
        {
            var parent = models
                .Where(x => x.Id == model?.Parent)
                .FirstOrDefault();
            if (parent == null)
                parent = models.Select(lst => FindParent(lst.Next, model)).FirstOrDefault();
            return parent;
        }


        #endregion CONTEST_F













        //###########################################################################################################################
        #region CONTEST_E

        private static void RunContestE()
        {
            var inputData1 = ReadIntArray(1, 100_000);

            int friendCount = inputData1[0];
            Dictionary<int, List<int>> friends = Enumerable.Range(1, friendCount).ToDictionary(k => k, card => new List<int>());

            int cardCount = inputData1[1];
            List<int> cards = Enumerable.Range(1, cardCount)
                .Reverse()
                .ToList();

            var inputData2 = ReadIntArray(1, cardCount);
            for (int i = 1; i <= friendCount; i++)
                friends[i] = Enumerable.Range(1, inputData2[i - 1]).ToList();

            var orderedFriends = friends
                .OrderByDescending(x => x.Value.Count())
                .ToDictionary(x => x.Key, x => x.Value);

            Dictionary<int, int> resultLst = new Dictionary<int, int>();
            foreach (var friend in orderedFriends)
            {
                var selectedCard = cards.Except(friend.Value).FirstOrDefault();
                resultLst.Add(friend.Key, selectedCard == 0 ? -1 : selectedCard);
                cards.Remove(selectedCard);
            }
            var orderedResultLst = resultLst
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value)
                .Values;

            Console.WriteLine(
                orderedResultLst.Contains(-1)
                ? -1
                : string.Join(" ", orderedResultLst)
                );
        }

        #endregion CONTEST_E





















        //###########################################################################################################################
        #region CONTEST_D

        private static void RunContestD()
        {
            int testCaseCount = ReadInt(1, 20);
            for (int i = 0; i < testCaseCount; i++)
            {
                RunCONTEST_D();
                Console.WriteLine();
            }

        }


        private static void RunCONTEST_D()
        {
            var inputData = ReadIntArray(1, 20);
            int k = inputData[0];
            int h = inputData[1];
            int w = inputData[2];

            List<List<char[]>> mounts = new List<List<char[]>>();
            for (int i = 0; i < k; i++)
            {
                List<char[]> mountReliefs = new List<char[]>();
                for (int j = 0; j < h; j++)
                    mountReliefs.Add(Console.ReadLine().ToCharArray());
                mounts.Add(mountReliefs);
                if (i != k - 1)
                    Console.ReadLine();
            }


            List<char[]> mountResult = mounts.First();
            foreach (var mount in mounts)
            {
                if (mount == mounts.First())
                    continue;

                for (int i = 0; i < mount.Count(); i++)
                    for (int j = 0; j < mount[i].Length; j++)
                    {
                        if (mountResult[i][j] == '.')
                            mountResult[i][j] = mount[i][j];
                    }
            }

            Console.WriteLine(string.Join(Environment.NewLine, mountResult.Select(i => string.Join("", i))));
        }

        #endregion // CONTEST_D
















        //###########################################################################################################################
        #region CONTEST_C

        private static void RunContestC()
        {
            int testCaseCount = ReadInt(1, 1000);
            for (int i = 0; i < testCaseCount; i++)
                RunTask();
        }

        private static void RunTask()
        {
            const int T_MAX = 30;
            const int T_MIN = 15;

            int employeeCount = ReadInt(1, 1000);

            List<string[]> inputStrings = new List<string[]>();
            for (int i = 0; i < employeeCount; i++)
                inputStrings.Add(Console.ReadLine()?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            List<int[]> temperatures = new List<int[]>();
            foreach (var item in inputStrings)
            {
                if (item.Length == 1)
                {
                    int.TryParse(item[0], out var temperature);
                    temperatures.Add(new int[2] { temperature, temperature });
                    continue;
                }
                int temperatureMin = T_MIN;
                int temperatureMax = T_MAX;
                int.TryParse(item[1], out var temperature2);

                switch (item[0])
                {
                    case ">":
                        temperatureMin = temperature2 + 1;
                        break;
                    case ">=":
                        temperatureMin = temperature2;
                        break;
                    case "=":
                        temperatureMin = temperature2;
                        temperatureMax = temperature2;
                        break;
                    case "==":
                        temperatureMin = temperature2;
                        temperatureMax = temperature2;
                        break;
                    case "<=":
                        temperatureMax = temperature2;
                        break;
                    case "<":
                        temperatureMax = temperature2 - 1;
                        break;
                }
                temperatures.Add(new int[2] { temperatureMin, temperatureMax });
            }

            var selectedDiap = temperatures.First();
            foreach (var diap in temperatures)
            {
                if (diap != selectedDiap)
                {
                    if (selectedDiap[0] < diap[0])
                        selectedDiap[0] = diap[0];
                    if (selectedDiap[1] > diap[1])
                        selectedDiap[1] = diap[1];
                }
                Console.WriteLine(
                    selectedDiap[1] < selectedDiap[0]
                    ? -1
                    : selectedDiap[0]
                    );

            }



        }

        #endregion // CONTEST_C


















        //###########################################################################################################################
        #region CONTESTB


        private static void RunContestB()
        {
            char[] resultString = Console.ReadLine().ToCharArray();
            int count = ReadInt(1, 1000);
            List<string[]> inputStrings = new List<string[]>();
            for (int i = 0; i < count; i++)
                inputStrings.Add(Console.ReadLine()?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            foreach (var s in inputStrings)
            {
                int.TryParse(s[0], out var start);
                int.TryParse(s[1], out var end);
                var str = s[2];
                var strLength = end - start + 1;

                for (int i = start - 1, j = 0; i < end; i++, j++)
                {
                    resultString[i] = str[j];
                }
            }
            Console.WriteLine(resultString);
        }


        #endregion CONTESTB










        //###########################################################################################################################
        #region TaskH

        private struct MapData
        {
            internal char Key;
            internal int[] Coords;
        }

        /// <summary>
        /// Map Validation
        /// </summary>
        private static void RunTaskH()
        {
            const int TEST_CASE_COUNT_MIN = 1;
            const int TEST_CASE_COUNT_MAX = 100;

            const int VALUE_MIN = 2;
            const int VALUE_MAX = 20;
            const int INPUT_DATA_COUNT = 2;

            var testCaseCount = ReadIntH(TEST_CASE_COUNT_MIN, TEST_CASE_COUNT_MAX)
                .First();

            for (int i = 0; i < testCaseCount; i++)
            {
                var inputData = ReadIntH(VALUE_MIN, VALUE_MAX);
                if (inputData.Length != INPUT_DATA_COUNT)
                    throw new ArgumentOutOfRangeException(nameof(inputData));
                var rowCount = inputData[0];
                var charCount = inputData[1];

                List<MapData> mapRows = new List<MapData>();
                for (int j = 0; j < rowCount; j++)
                    mapRows.AddRange(ReadMapLine(j, charCount));
                var mapLookup = mapRows
                    .ToLookup(pair => pair.Key, pair => pair.Coords);

                DisplayResultH2(mapLookup, rowCount, charCount);
            }
        }



        private static void DisplayResultH2(ILookup<char, int[]> mapLookup, int rowCount, int charCount)
        {
            const int ROW_FLAG = 10;

            bool isUnatedArea = true;
            foreach (var mapColor in mapLookup.AsParallel())
            {
                int[] koefs = GetKoefs(mapColor);

                if (koefs[0] >= ROW_FLAG
                    /*|| koefs[1] > ROW_FLAG * 2*/)
                {
                    isUnatedArea = false;
                    break;
                }


                var mapList1 = mapColor
                    .Select(item => item)
                    .OrderBy(i => i[1]).ThenBy(i => i[0])
                    .ToList();
                Dictionary<int, HashSet<int[]>> groups1 = new Dictionary<int, HashSet<int[]>>();
                int key1 = 0;
                foreach (var item in mapList1)
                {
                    var around = mapList1
                        .Where(x =>
                            (x[0] <= item[0] + ROW_FLAG && x[0] >= item[0] - ROW_FLAG)
                            && (x[1] <= item[1] + ROW_FLAG && x[1] >= item[1] - ROW_FLAG)
                            )
                        .ToHashSet();
                    if (groups1.Any(i => i.Value.Any(around.Contains)))
                        groups1.First(i => i.Value.Any(around.Contains)).Value.UnionWith(around);
                    else
                        groups1.Add(++key1, around);
                }

                var mapList2 = mapColor
                    .Select(item => item)
                    .OrderBy(i => i[0]).ThenBy(i => i[1])
                    .ToList();
                Dictionary<int, HashSet<int[]>> groups2 = new Dictionary<int, HashSet<int[]>>();
                int key2 = 0;
                foreach (var item in mapList2)
                {
                    var around = mapList2
                        .Where(x =>
                            (x[0] <= item[0] + ROW_FLAG && x[0] >= item[0] - ROW_FLAG)
                            && (x[1] <= item[1] + ROW_FLAG && x[1] >= item[1] - ROW_FLAG)
                            )
                        .ToHashSet();
                    if (groups2.Any(i => i.Value.Any(around.Contains)))
                        groups2.First(i => i.Value.Any(around.Contains)).Value.UnionWith(around);
                    else
                        groups2.Add(++key2, around);
                }

                //IEnumerable<int[]> xD1 = new List<int[]>();
                //foreach (var item in groups1.Values)
                //{
                //    xD1 = xD1.Except(item.ToList()).ToList();
                //}


                if (groups1.Count() > 1
                    && groups2.Count() > 1
                    && groups1.Values.Max(v => v.Count()) < mapList1.Count()
                    && groups2.Values.Max(v => v.Count()) < mapList2.Count()
                    )
                {
                    isUnatedArea = false;
                    break;
                }



                //if (koefs[1] >= COL_FLAG)
                //{
                //    var mapList = mapColor
                //        .Select(item => item)
                //        .ToList();
                //    Dictionary<int, HashSet<int[]>> groups = new Dictionary<int, HashSet<int[]>>();
                //    int key = 0;
                //    foreach (var item in mapList)
                //    {
                //        var around = mapList
                //            .Where(x =>
                //                (x[0] <= item[0] + ROW_FLAG && x[0] >= item[0] - ROW_FLAG)
                //                && (x[1] <= item[1] + ROW_FLAG && x[1] >= item[1] - ROW_FLAG)
                //                //&& (x != item)
                //                )
                //            .ToHashSet();
                //        if (groups.Any(i => i.Value.Any(around.Contains)))
                //            groups.First(i => i.Value.Any(around.Contains)).Value.UnionWith(around);
                //        else
                //            groups.Add(++key, around);
                //    }
                //if (groups.Count() > 1)
                //    {
                //        isUnatedArea = false;
                //        break;
                //    }
                //}
            }
            Console.WriteLine(isUnatedArea
                ? "YES"
                : "NO"
                );
        }

        private static int[] GetKoefs(IGrouping<char, int[]> mapColor)
        {


            var rowNumbers = mapColor
                .Select(c => c[0])
                .Distinct()
                .ToArray();
            var rowKoef = (rowNumbers.Max(r => r) - rowNumbers.Min(r => r)) / rowNumbers.Length;

            var colNumbers = mapColor
                .Select(c => c[1])
                .Distinct()
                .ToArray();
            var colDiap = (colNumbers.Max(r => r) - colNumbers.Min(r => r)) * 2 + 10;
            var colKoef = colDiap / colNumbers.Length + rowNumbers.Length;

            return new int[2] { rowKoef, colKoef };
        }



        //private static object RotateArray(string[][] array)
        //{
        //    string[][] rotatedArray = new string[array.Length][];
        //    for (int i = 0; i < rotatedArray.Length; i++)
        //    {
        //        for (int j = 0; j < rotatedArray.Length; j++)
        //        {
        //            rotatedArray[j][i] = array[array.Length - i - 1][j];
        //        }
        //    }
        //    return rotatedArray;
        //}



        //private static string[] ReadRowH(int charCount)
        //{
        //    var stringInputData = Console.ReadLine();
        //    if (stringInputData == null
        //        || stringInputData.Length > charCount
        //        )
        //        throw new ArgumentOutOfRangeException(nameof(stringInputData));

        //    return stringInputData
        //        .Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
        //        .ToArray();
        //}










        //private static void DisplayResultH(ILookup<char, int[]> mapLookup, int rowCount, int charCount)
        //{
        //    const int ROW_FLAG = 10;
        //    const int COL_DIFFERENCE_MAX = 30;

        //    bool isUnatedArea = true;
        //    foreach (var mapColor in mapLookup.AsParallel())
        //    {
        //        int rowKoef = GetKoef(mapColor);
        //        if (rowKoef >= ROW_FLAG)
        //        {
        //            isUnatedArea = false;
        //            break;
        //        }


        //        var rotatedMapColor = mapColor
        //        .Select(r =>
        //        {
        //            var tmp = r[0];
        //            r[0] = charCount * 10 - r[1] * 2 + 20;
        //            r[1] = tmp - (tmp / 10 - 1) * 5;
        //            return r;
        //        })
        //        .ToLookup(pair => mapColor.Key, arr => arr)
        //        .First()
        //        ;


        //        var colKoef = GetKoef(rotatedMapColor);
        //        if (colKoef >= ROW_FLAG * 2)
        //        {
        //            isUnatedArea = false;
        //            break;
        //        }
        //        if (colKoef >= ROW_FLAG)
        //        {
        //            var orderedMapColor1 = rotatedMapColor
        //                .OrderBy(x => x[0]).ThenByDescending(x => x[1])
        //                .ToArray();
        //            var orderedMapColor2 = rotatedMapColor
        //                .OrderBy(x => x[0]).ThenBy(x => x[1])
        //                .ToArray();

        //            int[] prevValue1 = new int[2];
        //            int[] prevValue2 = new int[2];
        //            for (var i = 0; i < orderedMapColor1.Length; i++)
        //            {
        //                if (i == 0)
        //                {
        //                    prevValue1 = orderedMapColor1[i];
        //                    prevValue2 = orderedMapColor2[i];
        //                    continue;
        //                }
        //                var difference1 = GetDifference(orderedMapColor1[i], prevValue1);
        //                var difference2 = GetDifference(orderedMapColor2[i], prevValue2);
        //                if (difference1 >= COL_DIFFERENCE_MAX
        //                    & difference2 >= COL_DIFFERENCE_MAX
        //                    )
        //                {
        //                    if (difference1 != difference2)
        //                    {
        //                        isUnatedArea = false;
        //                        break;
        //                    }
        //                }
        //                prevValue1 = orderedMapColor1[i];
        //                prevValue2 = orderedMapColor2[i];
        //            }
        //        }
        //    }
        //    Console.WriteLine(isUnatedArea
        //        ? "YES"
        //        : "NO"
        //        );
        //}

        //private static int GetDifference(int[] value, int[] prevValue)
        //{
        //    var difR = value[0] - prevValue[0];
        //    var difC = Math.Abs(value[1] - prevValue[1]);
        //    return difR + difC;
        //}

        //private static int GetKoef(IGrouping<char, int[]>? mapColor)
        //{
        //    var rowNumbers = mapColor.Select(c => c[0])
        //                        .Distinct()
        //                        .ToArray();
        //    var minRowNumber = rowNumbers.Min(r => r);
        //    var maxRowNumber = rowNumbers.Max(r => r);
        //    var rowKoef = (maxRowNumber - minRowNumber) / rowNumbers.Length;
        //    return rowKoef;
        //}


        private static IEnumerable<MapData> ReadMapLine(int rowNumber, int charCount)
        {
            var stringInputData = Console.ReadLine();
            if (stringInputData == null
                || stringInputData.Length > charCount
                )
                throw new ArgumentOutOfRangeException(nameof(stringInputData));

            rowNumber = (rowNumber + 1) * 10;
            var row = stringInputData
                .Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
                .Select((value, index) =>
                {
                    var key = value[0];
                    var indx = index + 1;
                    var colNumber = stringInputData[0] == '.'
                    ? (int)((indx + 0.5) * 10)
                    : indx * 10;
                    var coord = new int[2] { rowNumber, colNumber };
                    return new MapData { Key = key, Coords = coord };
                });
            return row;
        }


        /// <summary>
        /// Read string and convert to integer
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static int[] ReadIntH(int minValue, int maxValue)
        {
            var arrInt = Console.ReadLine()
                ?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    int.TryParse(s, out var n);
                    if (n < minValue
                        || n > maxValue
                        )
                        throw new ArgumentOutOfRangeException($"Value {n} out of range from {minValue} to {maxValue}");
                    return n;
                })
                .ToArray();
            if (arrInt == null)
                throw new ArgumentNullException($"Failed to get data from string");
            return arrInt;

        }

        #endregion // TaskH





        //###########################################################################################################################
        #region TaskG
        private static void RunTaskG()
        {
            const int INPUT_DATA_COUNT = 2;
            const int USER_COUNT_MIN = 2;

            const int PAIR_OF_FRIENDS_COUNT_MIN = 1;

            const int FRIENDS_COUNT_MAX = 5;

            var inputDataFirstLine = ReadIntF(0, int.MaxValue);
            if (inputDataFirstLine.Length != INPUT_DATA_COUNT)
                throw new ArgumentOutOfRangeException(nameof(inputDataFirstLine));

            int usersCount = GetUserCount(inputDataFirstLine, USER_COUNT_MIN);
            int numberOfFriendsPair = GetNumberOfFriendsPair(inputDataFirstLine, usersCount);

            var sw = new Stopwatch();
            sw.Start();

            List<int[]> pairOfFriemdsLst = new List<int[]>();
            for (int i = 0; i < numberOfFriendsPair; i++)
            {
                var pairOfFriemds = ReadIntF(PAIR_OF_FRIENDS_COUNT_MIN, usersCount);
                if (pairOfFriemds.Length != INPUT_DATA_COUNT
                    || pairOfFriemds.First() == pairOfFriemds.Last()
                    )
                    throw new ArgumentOutOfRangeException(nameof(pairOfFriemds));
                pairOfFriemdsLst.Add(pairOfFriemds);
                pairOfFriemdsLst.Add(pairOfFriemds.Reverse().ToArray());
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed); // Здесь логируем
            sw.Reset();

            sw.Start();
            var friendsLookup = pairOfFriemdsLst
                .OrderBy(p => p[0]).ThenBy(p => p[1])
                .ToLookup(pair => pair.First(), pair => pair.Last())
                .OrderBy(l => l.Key);
            if (friendsLookup.FirstOrDefault(fr => fr.Count() > FRIENDS_COUNT_MAX) != null)
                throw new ArgumentOutOfRangeException(nameof(friendsLookup));
            sw.Stop();
            Console.WriteLine(sw.Elapsed); // Здесь логируем
            sw.Reset();

            Dictionary<int, List<int>> mostPotentialFriends = Enumerable.Range(1, usersCount)
                .ToDictionary(k => k, u => new List<int>() { 0 });


            sw.Start();
            foreach (var item in friendsLookup.AsParallel())
            {
                var potentialFriends = friendsLookup
                    .Where(f => f.Contains(item.Key))
                    .SelectMany(f => f)
                    .Where(f => f != item.Key
                        && !item.Contains(f)
                    )
                    .GroupBy(f => f)
                    ;

                List<int> listWithLargestCount = new List<int>();
                if (!potentialFriends.Any())
                    listWithLargestCount.Add(0);
                else
                {
                    int maxCount = potentialFriends.Max(pf => pf.Count());
                    listWithLargestCount = potentialFriends
                        .Where(pf => pf.Count() == maxCount)
                        .Select(pf => pf.Key)
                        .OrderBy(pf => pf)
                        .ToList()
                        ;
                }
                mostPotentialFriends[item.Key] = listWithLargestCount;
            }
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine(sw.Elapsed); // Здесь логируем
            Console.ReadLine();
            Console.WriteLine(string.Join(Environment.NewLine, mostPotentialFriends.Select(f => string.Join(' ', f.Value))));
        }


        /// <summary>
        /// Get number of pair of friends
        /// </summary>
        /// <param name="inputDataFirstLine"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int GetNumberOfFriendsPair(int[] inputDataFirstLine, int usersCount)
        {
            const int NUMBEROF_PAIR_OF_FRIENDS_MIN = 0;

            var numberOfPairOfFriendsMax1 = (uint)(usersCount * (usersCount - 1) / 2);
            var numberOfPairOfFriendsMax2 = (5 * usersCount) / 2;
            var numberOfPairOfFriendsMax = Math.Min(numberOfPairOfFriendsMax1, numberOfPairOfFriendsMax2);
            var numberOfPairOfFriends = inputDataFirstLine[1];
            if (numberOfPairOfFriends < NUMBEROF_PAIR_OF_FRIENDS_MIN
                || numberOfPairOfFriends > numberOfPairOfFriendsMax
                )
                throw new ArgumentOutOfRangeException(nameof(numberOfPairOfFriends));
            return numberOfPairOfFriends;
        }

        /// <summary>
        /// Get number of users
        /// </summary>
        /// <param name="USER_COUNT_MIN"></param>
        /// <param name="inputDataFirstLine"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static int GetUserCount(int[] inputDataFirstLine, int minUsersCount)
        {
            const int USER_COUNT_MAX = 50_000;

            var usersCount = inputDataFirstLine[0];
            if (usersCount < minUsersCount
                || usersCount > USER_COUNT_MAX
                )
                throw new ArgumentOutOfRangeException(nameof(usersCount));
            return usersCount;
        }

        /// <summary>
        /// Read string and convert to integer
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static int[] ReadIntF(int minValue, int maxValue)
        {
            var arrInt = Console.ReadLine()
                ?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    int.TryParse(s, out var n);
                    if (n < minValue
                        || n > maxValue
                        )
                        throw new ArgumentOutOfRangeException($"Value {n} out of range from {minValue} to {maxValue}");
                    return n;
                })
                .ToArray();
            if (arrInt == null)
                throw new ArgumentNullException($"Failed to get data from string");
            return arrInt;

        }

        #endregion // TaskG



        //###########################################################################################################################
        #region TaskF

        private static void RunTaskF()
        {
            const int TEST_CASE_COUNT_MAX = 10;

            const int TIMESLOTS_COUNT_MIN = 1;
            const int TIMESLOTS_COUNT_MAX = 20_000;

            int.TryParse(Console.ReadLine(), out var testCaseCount);
            if (testCaseCount > TEST_CASE_COUNT_MAX)
                testCaseCount = TEST_CASE_COUNT_MAX;

            for (int i = 0; i < testCaseCount; i++)
            {
                if (!int.TryParse(Console.ReadLine(), out var timeslotsCount)
                    || timeslotsCount > TIMESLOTS_COUNT_MAX
                    || timeslotsCount < TIMESLOTS_COUNT_MIN
                    )
                    throw new ArgumentOutOfRangeException($"Number of timeslots ({timeslotsCount} is out of range (from {TIMESLOTS_COUNT_MIN} to {TIMESLOTS_COUNT_MAX}))");

                Console.WriteLine(ReadAndCheckTimeIntervals(timeslotsCount) ? "YES" : "NO");
            }

        }


        private static bool ReadAndCheckTimeIntervals(int timeslotsCount)
        {
            const int TIME_INTERVAL_VALUES = 2;

            bool isValid = true;

            Dictionary<TimeSpan, TimeSpan> timeslots = new Dictionary<TimeSpan, TimeSpan>();
            for (int i = 0; i < timeslotsCount; i++)
            {
                var timeslot = Console.ReadLine()
                    ?.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t =>
                    {
                        if (!TimeSpan.TryParse(t, out TimeSpan timeSpan)
                            || timeSpan.Days > 0
                            )
                        {
                            isValid = false;
                            return new TimeSpan();
                        }
                        return timeSpan;
                    })
                    .ToList();
                if (timeslot == null
                    || timeslot.Count != TIME_INTERVAL_VALUES
                    || timeslot.First() > timeslot.Last())
                {
                    isValid = false;
                    continue;
                }
                try
                {
                    timeslots.Add(timeslot.First(), timeslot.Last());
                }
                catch (Exception)
                {
                    isValid = false;
                    continue;
                }
            }
            return isValid ? CheckTimeIntervals(timeslots) : false;
        }

        /// <summary>
        /// Check time intervals;
        /// </summary>
        /// <param name="timeslots"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static bool CheckTimeIntervals(Dictionary<TimeSpan, TimeSpan> timeslots)
        {
            var orderedTimeslots = timeslots
                .OrderBy(t => t.Value)
                .ToDictionary(t => t.Key, t => t.Value)
                ;

            KeyValuePair<TimeSpan, TimeSpan> prevTimeslot = default;

            foreach (var timeslot in orderedTimeslots)
            {
                if (prevTimeslot.Equals(default(KeyValuePair<TimeSpan, TimeSpan>)))
                {
                    prevTimeslot = timeslot;
                    continue;
                }
                if (prevTimeslot.Value >= timeslot.Key)
                    return false;
                prevTimeslot = timeslot;
            }

            return true; ;
        }

        #endregion // TaskF



        //###########################################################################################################################
        #region TaskE

        private static void RunTaskE()
        {
            const int TEST_CASE_COUNT_MAX = 10;

            const int EMPLOYE_REPORT_COUNT_MIN = 3;
            const int EMPLOYE_REPORT_COUNT_MAX = 50_000;


            int testCaseCount = ReadIntE(1, TEST_CASE_COUNT_MAX).First();
            for (int i = 0; i < testCaseCount; i++)
            {
                var employeReportCount = ReadIntE(EMPLOYE_REPORT_COUNT_MIN, EMPLOYE_REPORT_COUNT_MAX).First();
                var employeReports = ReadIntE(1, employeReportCount);
                DisplayResultE(employeReports);
            }
        }

        /// <summary>
        /// Display result
        /// </summary>
        /// <param name="employeReports"></param>
        private static void DisplayResultE(int[] employeReports)
        {
            Console.WriteLine(CheckCriteria(employeReports)
                ? "YES"
                : "NO"
                );
        }


        private static bool CheckCriteria(int[] employeReports)
        {
            HashSet<int> uniquesReports = new HashSet<int>(employeReports);
            if (uniquesReports.Count() == employeReports.Length)
                return true;

            var lookup = employeReports
                .Select((value, index) => new { value, index })
                .ToLookup(pair => pair.value, pair => pair.index)
                .Where(item => item.Count() > 1
                    && item.First() != item.Last() - (item.Count() - 1)
                );
            return !lookup.Any();
        }


        /// <summary>
        /// Read string and convert to integer
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static int[] ReadIntE(int minValue, int maxValue)
        {
            var arrInt = Console.ReadLine()
                ?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    int.TryParse(s, out var n);
                    if (n < minValue
                        || n > maxValue
                        )
                        throw new ArgumentOutOfRangeException($"Value {n} out of range from {minValue} to {maxValue}");
                    return n;
                })
                .ToArray();
            if (arrInt == null)
                throw new ArgumentNullException($"Failed to get data from string");
            return arrInt;

        }

        #endregion // TaskE













        //###########################################################################################################################
        #region TaskD

        /// <summary>
        /// Spreadsheet
        /// </summary>
        private static void RunTaskD()
        {
            const int INPUT_DATA_SET_COUNT_MIN = 1;
            const int INPUT_DATA_SET_COUNT_MAX = 100;

            const int CILICK_COUNT_MIN = 1;
            const int CILICK_COUNT_MAX = 30;

            int.TryParse(Console.ReadLine(), out var inputDataSetCount);
            if (inputDataSetCount > INPUT_DATA_SET_COUNT_MAX
                || inputDataSetCount < INPUT_DATA_SET_COUNT_MIN
                )
                throw new ArgumentOutOfRangeException(nameof(inputDataSetCount));

            for (int i = 0; i < inputDataSetCount; i++)
            {
                Console.ReadLine();

                // - number of rows and columns
                int[] tableSize = ReadTableSize();

                // - values of table by row
                List<int[]> rows = new List<int[]>();
                for (int j = 0; j < tableSize[0]; j++)
                {
                    int[] row = ReadRow(tableSize[1]);
                    rows.Add(row);
                }

                // - clicks
                int clickCount = ReadIntD(CILICK_COUNT_MIN, CILICK_COUNT_MAX).First();
                int[] clickedColumnNumbers = ReadIntD(CILICK_COUNT_MIN, tableSize[1]);
                if (clickedColumnNumbers.Length != clickCount)
                    throw new ArgumentOutOfRangeException(nameof(clickedColumnNumbers));

                DisplaySortedTable(clickedColumnNumbers, rows);
            }
        }

        private static void DisplaySortedTable(int[] clickedColumnNumbers, List<int[]> rows)
        {
            for (int i = 0; i < clickedColumnNumbers.Length; i++)
                rows = rows.OrderBy(r => r[clickedColumnNumbers[i] - 1]).ToList();
            Console.WriteLine();
            Console.WriteLine(string.Join(Environment.NewLine, rows.Select(r => string.Join(' ', r))));

        }

        /// <summary>
        /// Read current row
        /// </summary>
        /// <param name="columnsCount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static int[] ReadRow(int columnsCount)
        {
            const int TABLE_ITEM_VALUE_MIN = 1;
            const int TABLE_ITEM_VALUE_MAX = 100;

            int[]? row = ReadIntD(TABLE_ITEM_VALUE_MIN, TABLE_ITEM_VALUE_MAX);
            if (row?.Length != columnsCount)
                throw new ArgumentOutOfRangeException($"Table must have a {columnsCount} columns");
            return row;
        }

        /// <summary>
        /// Read table sizes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int[] ReadTableSize()
        {
            const int VALUE_COUNT = 2;
            const int VALUE_MIN = 1;
            const int VALUE_MAX = 30;

            int[]? tableSize = ReadIntD(VALUE_MIN, VALUE_MAX);
            if (tableSize?.Length != VALUE_COUNT)
                throw new ArgumentOutOfRangeException($"Must be a {VALUE_COUNT}D table");
            return tableSize;
        }

        /// <summary>
        /// Read string and convert to integer
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static int[] ReadIntD(int minValue, int maxValue)
        {
            var arrInt = Console.ReadLine()
                ?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    int.TryParse(s, out var n);
                    if (n < minValue
                        || n > maxValue
                        )
                        throw new ArgumentOutOfRangeException($"Value {n} out of range from {minValue} to {maxValue}");
                    return n;
                })
                .ToArray();
            if (arrInt == null)
                throw new ArgumentNullException($"Failed to get data from string");
            return arrInt;

        }

        #endregion // TaskD



        //###########################################################################################################################
        #region TaskJ

        private static void RunTaskJ()
        {
            var wordDictionary = ReadDictionaryWords().ToArray();
            var wordRequests = ReadRequestWords().ToArray();
            var uniqueWordRequest = wordRequests.Distinct().ToArray();

            Random rnd = new Random();

            Dictionary<string, string> uniqueWordRequestRhymes = new Dictionary<string, string>();
            //int alternativeWordNumber = -1;
            //string alternativeWord = string.Empty;

            for (int i = 0; i < uniqueWordRequest.Length; i++)
            {
                var request = uniqueWordRequest[i];
                var selectedWords = GetRhymeWord(request, wordDictionary);
                if (string.IsNullOrEmpty(selectedWords))
                    uniqueWordRequestRhymes.Add(request, GetRandomWord(request, wordDictionary, rnd));
                else
                    uniqueWordRequestRhymes.Add(request, selectedWords);
            }

            //foreach (var request in uniqueWordRequest)
            //{
            //    var selectedWords = GetRhymeWord(request, wordDictionary);

            //    //if (string.IsNullOrEmpty(selectedWords))
            //    //{
            //    //    if (alternativeWordNumber + 1 >= wordDictionary.Length)
            //    //        alternativeWordNumber = -1;
            //    //    alternativeWord = wordDictionary[++alternativeWordNumber];
            //    //    if (alternativeWord == request)
            //    //    {
            //    //        if (alternativeWordNumber + 1 >= wordDictionary.Length)
            //    //            alternativeWordNumber = -1;
            //    //        alternativeWord = wordDictionary[++alternativeWordNumber];
            //    //    }
            //    //}

            //    var alternativeWord = GetRandomWord(request, wordDictionary, rnd);
            //    if (string.IsNullOrEmpty(selectedWords))
            //        uniqueWordRequestRhymes.Add(request, alternativeWord);
            //    else
            //        uniqueWordRequestRhymes.Add(request, selectedWords);
            //}

            List<string> resultLst = new List<string>();
            for (int i = 0; i < wordRequests.Length; i++)
                resultLst.Add(uniqueWordRequestRhymes[wordRequests[i]]);

            //foreach (var request in wordRequests)
            //    resultLst.Add(uniqueWordRequestRhymes[request]);

            Console.WriteLine(string.Join(Environment.NewLine, resultLst));
        }


        /// <summary>
        /// Read int value, which will be use as words array size
        /// </summary>
        /// <param name="dICTIONARY_SIZE_MIN"></param>
        /// <param name="dICTIONARY_SIZE_MAX"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int ReadSize(int minValue, int maxValue)
        {
            int.TryParse(Console.ReadLine(), out var size);
            if (size < minValue
                || size > maxValue
                )
                throw new ArgumentOutOfRangeException(
                    $"Value {size} out of range from {minValue} to {maxValue}"
                    );
            return size;
        }

        /// <summary>
        /// Read Words
        /// </summary>
        /// <param name="dICTIONARY_WORD_LENGTH_MIN"></param>
        /// <param name="dICTIONARY_WORD_LENGTH_MAX"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static string ReadWord(int minLength, int maxLength)
        {
            string pattern = @"^[a-z]+$";

            var word = Console.ReadLine();
            if (string.IsNullOrEmpty(word)
                || word.Length < minLength
                || word.Length > maxLength
                || !Regex.IsMatch(word, pattern)
                )
                throw new ArgumentException("The word does not match the format");

            return word;
        }

        /// <summary>
        /// Read dictionary of words
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> ReadDictionaryWords()
        {
            const int DICTIONARY_SIZE_MIN = 2;
            const int DICTIONARY_SIZE_MAX = 50_000;

            const int DICTIONARY_WORD_LENGTH_MIN = 1;
            const int DICTIONARY_WORD_LENGTH_MAX = 10;

            int wordDictionarySize = ReadSize(DICTIONARY_SIZE_MIN, DICTIONARY_SIZE_MAX);
            List<string> wordDictionary = new List<string>();
            for (int i = 0; i < wordDictionarySize; i++)
            {
                var word = ReadWord(DICTIONARY_WORD_LENGTH_MIN, DICTIONARY_WORD_LENGTH_MAX);
                if (wordDictionary.Contains(word))
                    throw new ArgumentException("The word is duplicate");
                wordDictionary.Add(word);
            }
            return wordDictionary;
        }

        /// <summary>
        /// Read request words
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> ReadRequestWords()
        {
            const int REQUEST_COUNT_MIN = 1;
            const int REQUEST_COUNT_MAX = 50_000;

            const int REQUEST_WORD_LENGTH_MIN = 1;
            const int REQUEST_WORD_LENGTH_MAX = 10;

            int wordRequestySize = ReadSize(REQUEST_COUNT_MIN, REQUEST_COUNT_MAX);
            List<string> requests = new List<string>();
            for (int i = 0; i < wordRequestySize; i++)
                requests.Add(ReadWord(REQUEST_WORD_LENGTH_MIN, REQUEST_WORD_LENGTH_MAX));

            return requests;
        }

        /// <summary>
        /// Get Rhyme word from source
        /// </summary>
        /// <param name="request">Rhyming word</param>
        /// <param name="wordDictionary">Source of words for rhymes</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static string GetRhymeWord(string request, IEnumerable<string> wordDictionary)
        {
            string? selectedWords = string.Empty;
            int coincidencesCountMax = 0;

            selectedWords = wordDictionary.FirstOrDefault(word =>
                word.EndsWith(request)
                && word != request
                );
            if (!string.IsNullOrEmpty(selectedWords))
                return selectedWords;


            foreach (var word in wordDictionary)
            {
                if (word == request)
                    continue;




                var coincidencesCount = 0;
                for (int i = word.Length - 1, j = request.Length - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (word[i] != request[j])
                        break;
                    coincidencesCount++;
                }
                if (coincidencesCount > coincidencesCountMax)
                {
                    coincidencesCountMax = coincidencesCount;
                    selectedWords = word;
                }
                if (coincidencesCountMax == request.Length)
                    break;
            }
            return selectedWords;



            //selectedWords = wordDictionary.FirstOrDefault((word, coincidencesCountMax) => 
            //    word != request 
            //    && (coincidencesCountMax 

            //       )
            //    );
        }

        private static string GetRandomWord(string request, string[] wordDictionary, Random rnd)
        {
            var alternativeWord = string.Empty;
            do
            {
                alternativeWord = wordDictionary[rnd.Next(0, wordDictionary.Count())];
            } while (alternativeWord == request);
            return alternativeWord;
        }

        #endregion // TaskJ




        //###########################################################################################################################
        #region TaskС

        private static void RunTaskC()
        {
            const int TEST_CASE_COUNT_MAX = 50;

            int.TryParse(Console.ReadLine(), out var testCaseCount);
            if (testCaseCount > TEST_CASE_COUNT_MAX)
                testCaseCount = TEST_CASE_COUNT_MAX;

            for (int i = 0; i < testCaseCount; i++)
            {
                var developers = GetDevelopers();
                var developerLevels = ReadDeveloperLevels(developers.Length);
                var teams = CreateTeams(developers, developerLevels);

                foreach (var team in teams)
                    Console.WriteLine(string.Join(" ", team.Value));
            }
        }

        /// <summary>
        /// Read the devlopers count
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static int[] GetDevelopers()
        {
            const int DEVELOPERS_COUNT_MIN = 2;
            const int DEVELOPERS_COUNT_MAX = 50;

            if (!int.TryParse(Console.ReadLine(), out var developersCount))
                throw new ArgumentException($"Invalid number of developers entered");
            if (developersCount > DEVELOPERS_COUNT_MAX
                || developersCount < DEVELOPERS_COUNT_MIN
                || developersCount % 2 != 0
                )
                throw new ArgumentOutOfRangeException(
                    $"Number of developers ({developersCount}) should be even and in the range from {DEVELOPERS_COUNT_MIN} to {DEVELOPERS_COUNT_MAX}"
                    );

            var developers = new int[developersCount];
            for (int i = 0; i < developersCount; i++)
                developers[i] = i + 1;
            return developers;
        }

        /// <summary>
        ///  Read the devlopers levels
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int[] ReadDeveloperLevels(int developersCount)
        {
            const int DEVELOPER_SKILL_LEVEL_MIN = 1;
            const int DEVELOPER_SKILL_LEVEL_MAX = 100;

            var developerLevels = Console.ReadLine()
                ?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    int.TryParse(s, out var lvl);
                    if (lvl < DEVELOPER_SKILL_LEVEL_MIN
                        || lvl > DEVELOPER_SKILL_LEVEL_MAX
                        )
                        throw new ArgumentOutOfRangeException($"Developer level {lvl} out of range from {DEVELOPER_SKILL_LEVEL_MIN} to {DEVELOPER_SKILL_LEVEL_MAX}");
                    return lvl;
                })
                .ToArray()
                ;
            if (developerLevels?.Length != developersCount)
                throw new ArgumentOutOfRangeException($"The number of developer levels {developerLevels?.Length} must be {developersCount}");
            return developerLevels;
        }

        /// <summary>
        /// Getting the next team member
        /// </summary>
        /// <param name="firstTeamMember"></param>
        /// <param name="leveledDevelopers"></param>
        /// <returns></returns>
        private static KeyValuePair<int, int> GetTeamMember(KeyValuePair<int, int> firstTeamMember, Dictionary<int, int> leveledDevelopers)
        {
            KeyValuePair<int, int> selectedLeveledDeveloper = default;
            int min = int.MaxValue;

            foreach (var developer in leveledDevelopers)
            {
                var value = Math.Abs(firstTeamMember.Value - developer.Value);
                if (value < min)
                {
                    min = value;
                    selectedLeveledDeveloper = developer;
                }
            }
            return selectedLeveledDeveloper;
        }

        /// <summary>
        /// Team building
        /// </summary>
        /// <param name = "developers" ></ param >
        /// < param name="developerLevels"></param>
        /// <returns></returns>
        /// <exception cref = "NotImplementedException" ></ exception >
        private static Dictionary<int, int[]> CreateTeams(int[] developers, int[] developerLevels)
        {
            const int developerCountInTeam = 2;

            Dictionary<int, int> leveledDevelopers = new Dictionary<int, int>();            // { developer Id, developer level } 
            for (int j = 0; j < developers.Length; j++)
                leveledDevelopers.Add(developers[j], developerLevels[j]);

            Dictionary<int, int[]> teams = new Dictionary<int, int[]>();

            int teamNumber = 0;
            while (leveledDevelopers.Count() > 0)
            {
                var firstTeamMember = leveledDevelopers.First();
                leveledDevelopers.Remove(firstTeamMember.Key);

                int[] team = new int[developerCountInTeam];
                team[0] = firstTeamMember.Key;
                for (int i = 1; i < developerCountInTeam; i++)
                {
                    var selectedLeveledDeveloper = GetTeamMember(firstTeamMember, leveledDevelopers);
                    team[i] = selectedLeveledDeveloper.Key;
                    leveledDevelopers.Remove(selectedLeveledDeveloper.Key);
                }
                teams.Add(++teamNumber, team);
            }
            return teams;
        }

        #endregion // TaskC




        //###########################################################################################################################
        #region TaskB

        private static void RunTaskB()
        {
            const int TEST_CASE_COUNT_MAX = 10_000;
            const int PRICE_MIN = 1;
            const int PRICE_MAX = 10_000;

            int.TryParse(Console.ReadLine(), out var testCaseCount);
            if (testCaseCount > TEST_CASE_COUNT_MAX)
                testCaseCount = TEST_CASE_COUNT_MAX;

            for (int i = 0; i < testCaseCount; i++)
            {
                var goodsCount = ReadGoodsCount();

                var enteringStringGoodsPrices = Console.ReadLine();
                var goodsPrices = enteringStringGoodsPrices
                    ?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s =>
                    {
                        int.TryParse(s, out var n);
                        if (n < PRICE_MIN
                         || n > PRICE_MAX
                         )
                            throw new ArgumentOutOfRangeException($"Price {n} out of range from {PRICE_MIN} to {PRICE_MAX}");
                        return n;
                    })
                    .ToArray()
                    ;
                if (goodsPrices?.Length != goodsCount)
                    throw new ArgumentOutOfRangeException($"The number of goods prices {goodsPrices?.Length} must be {goodsCount}");

                int totalAmount = GetTotalAmountWithDiscounts(goodsPrices);
                Console.WriteLine(totalAmount);
            }
        }

        /// <summary>
        /// Read the goods count
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the entered string is not a number</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the number of goods entered is out of range</exception>
        private static int ReadGoodsCount()
        {
            const int GOODS_COUNT_MIN = 1;
            const int GOODS_COUNT_MAX = 200_000;

            if (!int.TryParse(Console.ReadLine(), out var goodsCount))
                throw new ArgumentException($"Invalid number of goods entered");
            if (goodsCount > GOODS_COUNT_MAX
                || goodsCount < GOODS_COUNT_MIN
                )
                throw new ArgumentOutOfRangeException($"Number of goods ({goodsCount} is out of range (from {GOODS_COUNT_MIN} to {GOODS_COUNT_MAX}))");
            return goodsCount;
        }

        /// <summary>
        /// Calculation of the full price with discounts
        /// </summary>
        /// <param name="goodsPrices"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static int GetTotalAmountWithDiscounts(int[] goodsPrices)
        {
            const int FREE_ITEM_NUMBER = 3;

            int totalAmount = 0;
            var totalGoodsTypes = goodsPrices
                .GroupBy(p => p);

            foreach (var item in totalGoodsTypes)
            {
                var arr = item.Where((elt, indx) => (indx + 1) % FREE_ITEM_NUMBER != 0);
                totalAmount += arr.Sum();
            }
            return totalAmount;
        }

        #endregion // TaskB



        //###########################################################################################################################
        #region TaskA

        /// <summary>
        /// Сумматор
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static void RunTaskA()
        {
            const int TEST_CASE_COUNT_MAX = 10000;
            const int VALUE_MAX = 1000;
            const int VALUE_MIN = -1000;
            const int VALUE_COUNT = 2;

            int.TryParse(Console.ReadLine(), out var testCaseCount);
            if (testCaseCount > TEST_CASE_COUNT_MAX)
                testCaseCount = TEST_CASE_COUNT_MAX;

            for (int i = 0; i < testCaseCount; i++)
            {
                string? enteringString = Console.ReadLine();
                var numbers = enteringString
                    ?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s =>
                    {
                        int.TryParse(s, out var n);
                        if (n < VALUE_MIN
                         || n > VALUE_MAX
                         )
                            throw new ArgumentOutOfRangeException($"Value {n} out of range from {VALUE_MIN} to {VALUE_MAX}");
                        return n;
                    })
                    .ToArray()
                    ;
                if (numbers?.Length != VALUE_COUNT)
                    throw new ArgumentOutOfRangeException($"The number of values must be {VALUE_COUNT}");
                Console.WriteLine(numbers?.Sum());
            }
        }

        #endregion // TaskA



    }
}