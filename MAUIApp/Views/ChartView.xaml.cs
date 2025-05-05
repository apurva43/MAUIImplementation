namespace MAUIApp.Views;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
public partial class ChartView : ContentPage
{
	public ChartView()
	{
		InitializeComponent();
        LoadCharts();

    }
    void LoadCharts()
    {
        var dateLabels = new[]
        {
                "03-13", "03-14", "03-19", "03-21", "03-24", "03-31", "04-07", "04-14", "04-21", "04-28"
        };

        // Sample Data
        var solidsValues = new float[] { 23.8f, 23.5f, 23.6f, 23.5f, 23.4f, 23.9f, 24.2f, 24.5f, 23.9f, 24.0f };
        var phValues = new float[] { 5.85f, 5.80f, 5.78f, 5.76f, 5.72f, 5.75f, 5.84f, 5.84f, 5.82f, 5.83f };
        var solidsLSL = 20f;
        var solidsUSL = 24.5f;

        var phLSL = 5.4f;
        var phUSL = 6.1f;


        // % Solids Chart
        var solidsEntries = new List<ChartEntry>();
        for (int i = 0; i < solidsValues.Length; i++)
        {
            solidsEntries.Add(new ChartEntry(solidsValues[i])
            {
                Label = dateLabels[i],
                ValueLabel = solidsValues[i].ToString("F2"),
                Color = SKColor.Parse("#3b82f6")
            });
        }

        SolidsChartView.Chart = new LineChart
        {
            Entries = solidsEntries,
            LineMode = LineMode.Straight,
            LineSize = 5,
            PointMode = PointMode.Circle,
            PointSize = 10,
            LabelTextSize = 28,
            ValueLabelOption = ValueLabelOption.TopOfElement,
            BackgroundColor = SKColors.White
        };
        solidsEntries.Add(new ChartEntry(solidsLSL) { Color = SKColor.Parse("#f87171").WithAlpha(100) });
        solidsEntries.Add(new ChartEntry(solidsUSL) { Color = SKColor.Parse("#f87171").WithAlpha(100) });


        // pH Chart
        var phEntries = new List<ChartEntry>();
        for (int i = 0; i < phValues.Length; i++)
        {
            phEntries.Add(new ChartEntry(phValues[i])
            {
                Label = dateLabels[i],
                ValueLabel = phValues[i].ToString("F2"),
                //Color = SKColor.Parse("#1d4ed8")
            });
        }

        PhChartView.Chart = new LineChart
        {
            Entries = phEntries,
            LineMode = LineMode.Straight,
            LineSize = 5,
            PointMode = PointMode.Circle,
            PointSize = 10,
            LabelTextSize = 28,
            ValueLabelOption = ValueLabelOption.TopOfElement,
            BackgroundColor = SKColors.White
        };
        phEntries.Add(new ChartEntry(phLSL) { Color = SKColor.Parse("#fbbf24").WithAlpha(100) });
        phEntries.Add(new ChartEntry(phUSL) { Color = SKColor.Parse("#fbbf24").WithAlpha(100) });
    }
}
