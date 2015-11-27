var cpuChart;
var memChart;
var networkChart;
var driveCharts = [];
var netWorkChrts = [];
 var finalSeries = [];


Highcharts.theme = {
    global: {
        useUTC: false
    },
    colors: ["#DDDF0D", "#7798BF", "#55BF3B", "#DF5353", "#aaeeee", "#ff0066", "#eeaaee",
       "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
    chart: {
        backgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
            stops: [
               [0, 'rgb(96, 96, 96)'],
               [1, 'rgb(16, 16, 16)']
            ]
        },
        borderWidth: 0,
        borderRadius: 15,
        plotBackgroundColor: null,
        plotShadow: false,
        plotBorderWidth: 0
    },
    title: {
        style: {
            color: '#FFF',
            font: '16px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
        }
    },
    subtitle: {
        style: {
            color: '#DDD',
            font: '12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
        }
    },
    xAxis: {
        gridLineWidth: 0,
        lineColor: '#999',
        tickColor: '#999',
        labels: {
            style: {
                color: '#999',
                fontWeight: 'bold'
            }
        },
        title: {
            style: {
                color: '#AAA',
                font: 'bold 12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
            }
        }
    },
    yAxis: {
        alternateGridColor: null,
        minorTickInterval: null,
        gridLineColor: 'rgba(255, 255, 255, .1)',
        minorGridLineColor: 'rgba(255,255,255,0.07)',
        lineWidth: 0,
        tickWidth: 0,
        labels: {
            style: {
                color: '#999',
                fontWeight: 'bold'
            }
        },
        title: {
            style: {
                color: '#AAA',
                font: 'bold 12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
            }
        }
    },
    legend: {
        itemStyle: {
            color: '#CCC'
        },
        itemHoverStyle: {
            color: '#FFF'
        },
        itemHiddenStyle: {
            color: '#333'
        }
    },
    labels: {
        style: {
            color: '#CCC'
        }
    },
    tooltip: {
        backgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
            stops: [
               [0, 'rgba(96, 96, 96, .8)'],
               [1, 'rgba(16, 16, 16, .8)']
            ]
        },
        borderWidth: 0,
        style: {
            color: '#FFF'
        }
    },


    plotOptions: {
        series: {
            shadow: true
        },
        line: {
            dataLabels: {
                color: '#CCC'
            },
            marker: {
                lineColor: '#333'
            }
        },
        spline: {
            marker: {
                lineColor: '#333'
            }
        },
        scatter: {
            marker: {
                lineColor: '#333'
            }
        },
        candlestick: {
            lineColor: 'white'
        }
    },

    toolbar: {
        itemStyle: {
            color: '#CCC'
        }
    },

    navigation: {
        buttonOptions: {
            symbolStroke: '#DDDDDD',
            hoverSymbolStroke: '#FFFFFF',
            theme: {
                fill: {
                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                    stops: [
                       [0.4, '#606060'],
                       [0.6, '#333333']
                    ]
                },
                stroke: '#000000'
            }
        }
    },

    // scroll charts
    rangeSelector: {
        buttonTheme: {
            fill: {
                linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                stops: [
                   [0.4, '#888'],
                   [0.6, '#555']
                ]
            },
            stroke: '#000000',
            style: {
                color: '#CCC',
                fontWeight: 'bold'
            },
            states: {
                hover: {
                    fill: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                           [0.4, '#BBB'],
                           [0.6, '#888']
                        ]
                    },
                    stroke: '#000000',
                    style: {
                        color: 'white'
                    }
                },
                select: {
                    fill: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                           [0.1, '#000'],
                           [0.3, '#333']
                        ]
                    },
                    stroke: '#000000',
                    style: {
                        color: 'yellow'
                    }
                }
            }
        },
        inputStyle: {
            backgroundColor: '#333',
            color: 'silver'
        },
        labelStyle: {
            color: 'silver'
        }
    },

    navigator: {
        handles: {
            backgroundColor: '#666',
            borderColor: '#AAA'
        },
        outlineColor: '#CCC',
        maskFill: 'rgba(16, 16, 16, 0.5)',
        series: {
            color: '#7798BF',
            lineColor: '#A6C7ED'
        }
    },

    scrollbar: {
        barBackgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
            stops: [
               [0.4, '#888'],
               [0.6, '#555']
            ]
        },
        barBorderColor: '#CCC',
        buttonArrowColor: '#CCC',
        buttonBackgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
            stops: [
               [0.4, '#888'],
               [0.6, '#555']
            ]
        },
        buttonBorderColor: '#CCC',
        rifleColor: '#FFF',
        trackBackgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
            stops: [
               [0, '#000'],
               [1, '#333']
            ]
        },
        trackBorderColor: '#666'
    },

    // special colors for some of the demo examples
    legendBackgroundColor: 'rgba(48, 48, 48, 0.8)',
    legendBackgroundColorSolid: 'rgb(70, 70, 70)',
    dataLabelsColor: '#444',
    textColor: '#E0E0E0',
    maskColor: 'rgba(255,255,255,0.3)'
};

$(document).ready(function () {

    Highcharts.setOptions(Highcharts.theme);

    InitializeCPUGraph();
    InitializeMemoryGraph();
 
    
    for (var i = 0; i < drivePieChartData.length; i++) {
        AddDrivePieChart(drivePieChartData[i].IpAddress, [{
            x: 'TS',
            y: drivePieChartData[i].DriveTotalSpace
        }, {
            x: 'AS',
            y: drivePieChartData[i].DriveSpaceAvailable
        }]);
    }
    var SeriesObj = [];
  
    var unique = {};
    var distinct = [];
    for (var i in networkSeries) {
        if (typeof (unique[networkSeries[i].id]) == "undefined") {
            if (networkSeries[i].id)
            { distinct.push(networkSeries[i].id); }
            
        }
        unique[networkSeries[i].id] = 0;
    }
   
    for (var i = 0; i < distinct.length; i++)
    {   
        var obj = [];
        for (var j = 0; j < networkSeries.length; j++) {

            if (networkSeries[j].id == distinct[i])
            {
                obj.push(networkSeries[j]);
            }
        }
        finalSeries.push({ id: distinct[i], SeriesDistinct: obj,index:i});
    }
    var temp = finalSeries;

    for (var i = 0; i < finalSeries.length; i++) {

        InitializeNetworkGraph(finalSeries[i]);
    }
         

});

function isEven(value) {
    if (value % 2 == 0)
        return true;
    else
        return false;
}

function InitializeNetworkGraph(nSeries) {

    var element = document.getElementById('networkChart' + nSeries.id);
    if (typeof (element) != 'undefined' && element != null) {

    }
    else {
        if (isEven(nSeries.index))
        {
            $('#networkGraph').append('<div id="networkChart' + nSeries.id + '"style="min-width: 310px;flex:1; height: 300px;width: 650px; margin:10px;margin-left: 1px;"></div>');
        }
        else
        {
            $('#networkGraph').append('<div id="networkChart' + nSeries.id + '"style="min-width: 310px; height:  300px;width: 650px; margin:10px;margin-left: 1px;"></div>');
        }
        
    }

   
    var networkChart = new Highcharts.Chart({
        chart: {
            renderTo: 'networkChart' + nSeries.id,
            type: 'column',
            animation: Highcharts.svg, // don't animate in old IE
            events: {
                load: function () {
                    startNetworkUpdate();
                }
            }
        },
        title: {
            text: nSeries.id+'  Network Performance'
        },
        xAxis: {
            type: 'datetime',
            tickPixelInterval: 150
        },
        yAxis: {
            title: {
                text: 'Network Utilization'
            },
            min: 0,
            max: 100,
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.series.name + '</b><br/>' +
                'Name:' + this.series.userOptions.servername + '<br/>' +
                'TotalUsage:'+Highcharts.numberFormat(this.y, 2);
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        exporting: {
            enabled: false
        },

        series: nSeries.SeriesDistinct
    });
    netWorkChrts.push({ chart: networkChart, id: nSeries.id });

}

function InitializeCPUGraph() {

    //  cpuChart = new Highcharts.Chart({
    //    chart: {
    //        type: 'column',
    //        renderTo: 'cpuGraphss',
    //        animation: true, // don't animate in old IE
    //        events: {
    //            load: function () {
    //              //  startUpdate();
    //            }
    //        }
    //    },
    //    title: {
    //        text: 'Live CPU Performance'
    //    },
    //    xAxis: {
    //        type: 'datetime',
    //        tickPixelInterval: 150
    //    },
    //    yAxis: {
    //        min: 0,
    //        max: 100,
    //        title: {
    //            text: 'Value'
    //        },
    //    },
    //    tooltip: {
    //        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
    //        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
    //            '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
    //        footerFormat: '</table>',
    //        shared: true,
    //        useHTML: true,
          

    //    },
    //    plotOptions: {
    //        column: {
    //            pointPadding: 0.2,
    //            borderWidth: 0
    //        }
    //    },
    //    exporting: {
    //        enabled: false
    //    },

    //    series: cpuSeries
    //});




    cpuChart = new Highcharts.Chart({
        chart: {
            renderTo: 'cpuGraph',
            type: 'column',
            animation: Highcharts.svg, // don't animate in old IE
            events: {
                load: function () {
                    startUpdate();
                }
            }
        },
        title: {
            text: 'Live CPU Performance'
        },
        xAxis: {
            type: 'datetime',
            tickPixelInterval: 150
        },
        yAxis: {
            title: {
                text: 'CPU Usage'
            },
            min: 0,
            max: 100,
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.series.name + '</b><br/>' +
                Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '<br/>' +
                Highcharts.numberFormat(this.y, 2);
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        exporting: {
            enabled: false
        },
        
        series: cpuSeries
    });
    console.log('here')
    console.log(cpuSeries)

}


function InitializeMemoryGraph() {

    memChart = new Highcharts.Chart({
        chart: {
            renderTo: 'memoryGraph',
            type: 'column',
            animation: Highcharts.svg, // don't animate in old IE
            events: {
                load: function () {
                    startUpdate();
                }
            }
        },
        title: {
            text: 'Live Memory Performance'
        },
        xAxis: {
            type: 'datetime',
            tickPixelInterval: 150
        },
        yAxis: {
            title: {
                text: 'Memory Usage'
            },
            min: 0,
            max: 100,
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.series.name + '</b><br/>' +
                Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '<br/>' +
                Highcharts.numberFormat(this.y, 2);
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        exporting: {
            enabled: false
        },
        series: memSeries
    });
}


function AddDrivePieChart(ipAddress, data) {
    var check = ipAddress.split(":");
    var bit = 0;
    for (var i = 0; i < driveCharts.length; i++) {
        var ip = driveCharts[i].ipAddress.split(":");
        if (check[0] == ip[0]) {
            bit = 1;
        }
    }
    if (bit == 1) {
        $('#driveInfo').append('<div id="driveChart' + driveCharts.length + '" style="float:left;min-width: 310px;padding-left: 5px; padding-top: 10px;height: 200px; margin: 0 auto"></div>');
    }
    else {
        if (driveCharts.length != 0) {
            $('#driveInfo').append('<div id="driveChart' + driveCharts.length + '" style="float:left;clear:both;min-width: 310px;padding-left: 5px;padding-top: 10px; height: 200px; margin: 0 auto"></div>');
        }
        else {
            $('#driveInfo').append('<div id="driveChart' + driveCharts.length + '" style="float:left;clear:both;min-width: 310px; height: 200px;padding-left: 5px; padding-top: 10px;margin: 0 auto"></div>');
        }
    }
    var driveChart = new Highcharts.Chart({
        chart: {
            renderTo: 'driveChart' + driveCharts.length,
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            events: {
                load: function () {
                    startDriveUpdate();
                }
            }
        },
        title: {
            text: 'Drive Space On ' + ipAddress
        },
        tooltip: {
            pointFormat: '<b>{point.x}</b>:{point.y:.1f} ({point.percentage:.1f} %)'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#ffffff',
                    connectorColor: '#ffffff',
                    format: '{point.x}: <b>{point.percentage:.1f}%</b>'
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Drive Space',
            data: data
        }]
    });

    if (driveCharts.length > 0) {
        var a = 0;
        for (var i = 0; i < driveCharts.length; i++) {
            if (driveCharts[i].ipAddress == ipAddress) {
                a = 1;
                driveCharts[i].chart.series[0].points[0].y = data[0].y;
                driveCharts[i].chart.series[0].points[1].y = data[1].y;
            }
        }
        if (a == 0) {
            driveCharts.push({ chart: driveChart, ipAddress: ipAddress });
        }
    }
    else {
        driveCharts.push({ chart: driveChart, ipAddress: ipAddress });
    }

}



