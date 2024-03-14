using NPOI.SS.UserModel;
using WeatherViewer.Models.DBEntities;

namespace WeatherViewer.Services
{
    public static class ParseService
    {
        public static WeatherArchiveRecord ParseRow(IRow tableRow)
        {
            // todo : validate
            //var isRowValid = IsRowValid(tableRow);
            //if (!isRowValid)
            //{
            //    throw new Exception("Некорректные входные данные!");
            //}

            var result = new WeatherArchiveRecord()
            {
                Created = DateTime.Parse($"{GetCellValue(tableRow.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK))} {GetCellValue(tableRow.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK))}"),
                Temperature = GetCellValueFromRow<decimal?>(tableRow, 2),
                Humidity = GetCellValueFromRow<decimal?>(tableRow, 3),
                DewPoint = GetCellValueFromRow<decimal?>(tableRow, 4),
                Pressure = GetCellValueFromRow<short?>(tableRow, 5),
                WindDirection = GetCellValueFromRow<string?>(tableRow, 6),
                WindSpeed = GetCellValueFromRow<byte?>(tableRow, 7),
                Cloudiness = GetCellValueFromRow<byte?>(tableRow, 8),
                CloudBase = GetCellValueFromRow<short?>(tableRow, 9),
                HorizontalVisibility = GetCellValueFromRow<string?>(tableRow, 10),
                WeatherСonditions = GetCellValueFromRow<string?>(tableRow, 11)
            };

            return result;
        }

        private static T? GetCellValueFromRow<T>(IRow row, int cellIndex)
        {
            var cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_NULL_AND_BLANK);
            var cellValue = GetCellValue(cell);
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                return default;
            }
            return cellValue.ChangeType<T>();
        }

        private static string GetCellValue(ICell cell)
        {
            if (cell is null)
            {
                return string.Empty;
            }

            // todo : check for other types
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                default:
                    return string.Empty;
            }
        }

        private static T? ChangeType<T>(this object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }
    }
}
