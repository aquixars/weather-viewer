using System;
using System.Collections.Generic;

namespace WeatherViewer.Models.DBEntities;

public partial class WeatherArchiveRecord
{
    public long Id { get; set; }

    public DateTime? Created { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Humidity { get; set; }

    public short? Pressure { get; set; }

    public decimal? DewPoint { get; set; }

    public string? WindDirection { get; set; }

    public byte? WindSpeed { get; set; }

    public byte? Cloudiness { get; set; }

    public short? CloudBase { get; set; }

    public string? HorizontalVisibility { get; set; }

    public string? WeatherСonditions { get; set; }
}
