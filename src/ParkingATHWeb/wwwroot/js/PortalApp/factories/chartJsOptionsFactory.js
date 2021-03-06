﻿(function () {
    'use strict';

    function chartJsOptionsFactory() {

       function getDefaultLineOptions() {
           return {
                scaleShowGridLines: true, ///Boolean - Whether grid lines are shown across the chart		
                scaleGridLineColor: "#132d4b", //String - Colour of the grid lines		
                scaleGridLineWidth: 1, //Number - Width of the grid lines		
                scaleShowHorizontalLines: true, //Boolean - Whether to show horizontal lines (except X axis)		
                scaleShowVerticalLines: false, //Boolean - Whether to show vertical lines (except Y axis)		
                bezierCurve: true, //Boolean - Whether the line is curved between points		
                bezierCurveTension: 0.4, //Number - Tension of the bezier curve between points		
                pointDot: true, //Boolean - Whether to show a dot for each point		
                pointDotRadius: 5, //Number - Radius of each point dot in pixels		
                pointDotStrokeWidth: 2, //Number - Pixel width of point dot stroke		
                pointHitDetectionRadius: 20, //Number - amount extra to add to the radius to cater for hit detection outside the drawn point		
                datasetStroke: true, //Boolean - Whether to show a stroke for datasets		
                datasetStrokeWidth: 3, //Number - Pixel width of dataset stroke		
                datasetFill: true, //Boolean - Whether to fill the dataset with a colour				
                animationSteps: 15, // Number - Number of animation steps		
                animationEasing: "easeOutQuart", // String - Animation easing effect			
                scaleFontSize: 12, // Number - Scale label font size in pixels		
                scaleFontStyle: "normal", // String - Scale label font weight style		
                scaleFontColor: "#132d4b", // String - Scale label font colour
                tooltipEvents: ["mousemove", "touchstart", "touchmove"], // Array - Array of string names to attach tooltip events		
                tooltipFillColor: "#4b85c5", // String - Tooltip background colour		
                tooltipTitleFontFamily: "'Roboto','Helvetica Neue', 'Helvetica', 'Arial', sans-serif", // String - Tooltip title font declaration for the scale label		
                tooltipFontSize: 12, // Number - Tooltip label font size in pixels
                tooltipFontColor: "white", // String - Tooltip label font colour		
                tooltipTitleFontSize: 14, // Number - Tooltip title font size in pixels		
                tooltipTitleFontStyle: "bold", // String - Tooltip title font weight style		
                tooltipTitleFontColor: "#2e6cb2", // String - Tooltip title font colour		
                tooltipYPadding: 8, // Number - pixel width of padding around tooltip text		
                tooltipXPadding: 16, // Number - pixel width of padding around tooltip text		
                tooltipCaretSize: 10, // Number - Size of the caret on the tooltip		
                tooltipCornerRadius: 6, // Number - Pixel radius of the tooltip border		
                tooltipXOffset: 10, // Number - Pixel offset from point x to tooltip edge
                responsive: true
            }
       }

        return {
            getDefaultLineOptions: getDefaultLineOptions
        };
    }

    angular.module('portalApp').factory('chartJsOptionsFactory', chartJsOptionsFactory);
})();