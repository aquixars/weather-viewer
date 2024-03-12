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

            var cells = tableRow.Cells;

            // todo : culture
            var result = new WeatherArchiveRecord()
            {
                Created = DateTime.Parse($"{GetCellValue(cells[0])} {GetCellValue(cells[1])}"),
                Temperature = decimal.Parse(GetCellValue(cells[2])),
                Humidity = decimal.Parse(GetCellValue(cells[3])),
                DewPoint = decimal.Parse(GetCellValue(cells[4])),
                Pressure = short.Parse(GetCellValue(cells[5])),
                WindDirection = GetCellValue(cells[6]),
                WindSpeed = byte.Parse(GetCellValue(cells[7])),
                Cloudiness = byte.Parse(GetCellValue(cells[8])),
                CloudBase = short.Parse(GetCellValue(cells[9])),
                HorizontalVisibility = short.Parse(GetCellValue(cells[10])),
                WeatherСonditions = GetCellValue(cells[11])
            };

            return result;
        }

        private static string GetCellValue(ICell cell)
        {
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
