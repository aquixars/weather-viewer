using NPOI.SS.UserModel;
using TestTasks.DS.WeatherViewer.Models.DBEntities;

namespace TestTasks.DS.WeatherViewer.Services
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

            // todo : culture
            // todo : fix code spam
            var result = new WeatherArchiveRecord()
            {
                Created = DateTime.Parse($"{GetCellValue(tableRow.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK))} {GetCellValue(tableRow.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK))}"),
                Temperature = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : decimal.Parse(GetCellValue(tableRow.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK))),
                Humidity = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(3, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : decimal.Parse(GetCellValue(tableRow.GetCell(3, MissingCellPolicy.RETURN_NULL_AND_BLANK))),
                DewPoint = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(4, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : decimal.Parse(GetCellValue(tableRow.GetCell(4, MissingCellPolicy.RETURN_NULL_AND_BLANK))),
                Pressure = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(5, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : short.Parse(GetCellValue(tableRow.GetCell(5, MissingCellPolicy.RETURN_NULL_AND_BLANK))),
                WindDirection = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(6, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : GetCellValue(tableRow.GetCell(6, MissingCellPolicy.RETURN_NULL_AND_BLANK)),
                WindSpeed = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(7, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : byte.Parse(GetCellValue(tableRow.GetCell(7, MissingCellPolicy.RETURN_NULL_AND_BLANK))),
                Cloudiness = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(8, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : byte.Parse(GetCellValue(tableRow.GetCell(8, MissingCellPolicy.RETURN_NULL_AND_BLANK))),
                CloudBase = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(9, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : short.Parse(GetCellValue(tableRow.GetCell(9, MissingCellPolicy.RETURN_NULL_AND_BLANK))),
                HorizontalVisibility = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(10, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : GetCellValue(tableRow.GetCell(10, MissingCellPolicy.RETURN_NULL_AND_BLANK)),
                WeatherСonditions = string.IsNullOrWhiteSpace(GetCellValue(tableRow.GetCell(11, MissingCellPolicy.RETURN_NULL_AND_BLANK))) ? null : GetCellValue(tableRow.GetCell(11, MissingCellPolicy.RETURN_NULL_AND_BLANK))
            };

            return result;
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
    }
}
