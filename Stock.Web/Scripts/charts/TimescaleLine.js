function TimescaleLine(params) {

    'use strict';

    //[Meta].
    var self = this;
    self.TimescaleLine = true;

    //Properties.
    self.parent = params.parent;
    self.chart = parent;

    //UI.
    self.ui = (function () {

        var frame = document.getElementById(params.timelineFrameId);
        var pointer = document.getElementById(params.timelinePointerId);
        var borderWeight = params.timelinePointerBorderWeight;
        $(pointer).css('border', borderWeight + 'px solid red');

        function getSizeAndPosition() {
            var framePosition = $(frame).position();
            var pointerPosition = $(pointer).position();
            return {
                frameLeft: framePosition.left,
                frameTop: framePosition.top,
                frameWidth: $(frame).width(),
                frameHeight: $(frame).height(),
                pointerLeft: pointerPosition.left,
                pointerTop: pointerPosition.top,
                pointerWidth: $(pointer).width(),
                pointerHeight: $(pointer).height()
            }
        }

        function adjustLayout(params) {
            var layoutData = getSizeAndPosition();
            var wholeWidth = layoutData.frameWidth;
            var pointerWidth = wholeWidth * params.visiblePart + 2 * borderWeight;
            var pointerLeft = wholeWidth * params.leftOffset + layoutData.frameLeft - borderWeight;

            $(pointer).width(pointerWidth);
            $(pointer).css('left', pointerLeft + 'px');

        }

        return {
            frame: frame,
            pointer: pointer,
            getSizeAndPosition: getSizeAndPosition,
            adjustLayout: adjustLayout
        };

    })();

    //Events.
    (function bindEvents() {
        self.parent.bind({
            postRender: function (e) {
                adjustElementsAfterChartRendering(e.params);
            }
        });
    })();


    function adjustElementsAfterChartRendering(params) {
        var svgWidth = params.svgWidth;
        var viewWidth = params.viewWidth;
        var visiblePart = viewWidth / svgWidth;
        var leftOffset = params.left / svgWidth;

        self.ui.adjustLayout({
            leftOffset: leftOffset,
            visiblePart: visiblePart
        });

    }

}
TimescaleLine.prototype.bind = function (e) {
    $(self).bind(e);
}
TimescaleLine.prototype.trigger = function (e) {
    $(self).trigger(e);
}