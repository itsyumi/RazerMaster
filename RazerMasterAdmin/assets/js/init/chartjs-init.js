(function ($) {
    "use strict";

    //Sales chart
    //var ctx = document.getElementById("sales-chart");
    //ctx.height = 150;
    //var myChart = new Chart(ctx, {
    //    type: 'line',
    //    data: {
    //        labels: ["2012", "2013", "2014", "2015", "2016", "2017", "2018"],
    //        type: 'line',
    //        defaultFontFamily: 'Montserrat',
    //        datasets: [{
    //            label: "Foods",
    //            data: [0, 30, 15, 110, 50, 63, 120],
    //            backgroundColor: 'transparent',
    //            borderColor: 'rgba(220,53,69,0.75)',
    //            borderWidth: 3,
    //            pointStyle: 'circle',
    //            pointRadius: 5,
    //            pointBorderColor: 'transparent',
    //            pointBackgroundColor: 'rgba(220,53,69,0.75)',
    //        }, {
    //            label: "Electronics",
    //            data: [0, 50, 40, 80, 35, 99, 80],
    //            backgroundColor: 'transparent',
    //            borderColor: 'rgba(40,167,69,0.75)',
    //            borderWidth: 3,
    //            pointStyle: 'circle',
    //            pointRadius: 5,
    //            pointBorderColor: 'transparent',
    //            pointBackgroundColor: 'rgba(40,167,69,0.75)',
    //        }]
    //    },
    //    options: {
    //        responsive: true,

    //        tooltips: {
    //            mode: 'index',
    //            titleFontSize: 12,
    //            titleFontColor: '#000',
    //            bodyFontColor: '#000',
    //            backgroundColor: '#fff',
    //            titleFontFamily: 'Montserrat',
    //            bodyFontFamily: 'Montserrat',
    //            cornerRadius: 3,
    //            intersect: false,
    //        },
    //        legend: {
    //            display: false,
    //            labels: {
    //                usePointStyle: true,
    //                fontFamily: 'Montserrat',
    //            },
    //        },
    //        scales: {
    //            xAxes: [{
    //                display: true,
    //                gridLines: {
    //                    display: false,
    //                    drawBorder: false
    //                },
    //                scaleLabel: {
    //                    display: false,
    //                    labelString: 'Month'
    //                }
    //            }],
    //            yAxes: [{
    //                display: true,
    //                gridLines: {
    //                    display: false,
    //                    drawBorder: false
    //                },
    //                scaleLabel: {
    //                    display: true,
    //                    labelString: 'Value'
    //                }
    //            }]
    //        },
    //        title: {
    //            display: false,
    //            text: 'Normal Legend'
    //        }
    //    }
    //});

    //line chart
    var ctx = document.getElementById("lineChart");
    ctx.height = 150;
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ["January", "February", "March", "April", "May", "June", "July"],
            datasets: [
                {
                    label: "Mice",
                    borderColor: "rgba(0,0,0,.09)",
                    borderWidth: "1",
                    backgroundColor: "rgba(0,0,0,.07)",
                    data: [20, 47, 35, 43, 65, 45, 35]
                },
                {
                    label: "KeyBoard",
                    borderColor: "rgba(0, 194, 146, 0.9)",
                    borderWidth: "1",
                    backgroundColor: "rgba(0, 194, 146, 0.5)",
                    pointHighlightStroke: "rgba(220,220,220,1)",
                    data: [16, 32, 18, 27, 42, 33, 44]
                }
            ]
        },
        options: {
            responsive: true,
            tooltips: {
                mode: 'index',
                intersect: false
            },
            hover: {
                mode: 'nearest',
                intersect: true
            }
        }
    });


    //bar chart
    var ctx = document.getElementById("barChart");
    //    ctx.height = 200;
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: @Html.Raw(Json.Encode(reMonthANLS.Month)),
            datasets: [
                {
                    label: "First Purchase",
                    data: [65, 59, 80, 81, 56, 55, 45],
                    borderColor: "rgba(0, 194, 146, 0.9)",
                    borderWidth: "0",
                    backgroundColor: "rgba(0, 194, 146, 0.5)"
                },
                {
                    label: "Repurchase",
                    data: [28, 48, 40, 19, 86, 27, 76],
                    borderColor: "rgba(0,0,0,0.09)",
                    borderWidth: "0",
                    backgroundColor: "rgba(0,0,0,0.07)"
                }
            ]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });

    //pie chart
    var ctx = document.getElementById("pieChart");
    ctx.height = 150;
    var myChart = new Chart(ctx, {
        type: 'pie',
        data: {
            datasets: [{
                data: [45, 25, 20, 10],
                backgroundColor: [
                    "rgba(0, 194, 146,0.9)",
                    "rgba(0, 194, 146,0.7)",
                    "rgba(0, 194, 146,0.5)",
                    "rgba(0,0,0,0.07)"
                ],
                hoverBackgroundColor: [
                    "rgba(0, 194, 146,0.9)",
                    "rgba(0, 194, 146,0.7)",
                    "rgba(0, 194, 146,0.5)",
                    "rgba(0,0,0,0.07)"
                ]

            }],
            labels: [
                "June",
                "July",
                "Aug"
            ]
        },
        options: {
            responsive: true
        }
    });

    //doughut chart
    //var ctx = document.getElementById("doughutChart");
    //ctx.height = 150;
    //var myChart = new Chart(ctx, {
    //    type: 'doughnut',
    //    data: {
    //        datasets: [{
    //            data: [35, 40, 20, 5],
    //            backgroundColor: [
    //                "rgba(0, 194, 146,0.9)",
    //                "rgba(0, 194, 146,0.7)",
    //                "rgba(0, 194, 146,0.5)",
    //                "rgba(0,0,0,0.07)"
    //            ],
    //            hoverBackgroundColor: [
    //                "rgba(0, 194, 146,0.9)",
    //                "rgba(0, 194, 146,0.7)",
    //                "rgba(0, 194, 146,0.5)",
    //                "rgba(0,0,0,0.07)"
    //            ]

    //        }],
    //        labels: [
    //            "May",
    //            "June",
    //            "July",
    //            "Aug"
    //        ]
    //    },
    //    options: {
    //        responsive: true
    //    }
    //});


    // single bar chart
    var ctx = document.getElementById("singelBarChart");
    ctx.height = 150;
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["Sun", "Mon", "Tu", "Wed", "Th", "Fri", "Sat"],
            datasets: [
                {
                    label: "Sales Volume",
                    data: [55, 50, 75, 80, 56, 55, 60],
                    borderColor: "rgba(0, 194, 146, 0.9)",
                    borderWidth: "0",
                    backgroundColor: "rgba(0, 194, 146, 0.5)"
                }
            ]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });




})(jQuery);