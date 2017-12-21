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

        var pointerOffsetAgainstFrame = 1;
        var frame = document.getElementById(params.timelineFrameId);
        var pointerContainer = document.getElementById(params.timelinePointerContainerId);
        var pointerBorder = document.getElementById(params.timelinePointerBorderId);
        var pointerInside = document.getElementById(params.timelinePointerInsideId);
        var pointerLeftExpander = document.getElementById(params.timelinePointerLeftExpanderId);
        var pointerRightExpander = document.getElementById(params.timelinePointerRightExpanderId);
        //Moving & resizing state.
        var minWidth = 50;
        var moveMode = false;
        var rightResizeMode = false;
        var leftResizeMode = false;
        var cursorX = 0;

        function getSizeAndPosition() {
            var framePosition = $(frame).position();
            var pointerPosition = $(pointerContainer).position();
            return {
                frameLeft: framePosition.left,
                frameTop: framePosition.top,
                frameWidth: $(frame).width(),
                frameHeight: $(frame).height(),
                pointerLeft: pointerPosition.left,
                pointerTop: pointerPosition.top,
                pointerWidth: $(pointerContainer).width(),
                pointerHeight: $(pointerContainer).height()
            }
        }

        function adjustLayout(params) {
            var layoutData = getSizeAndPosition();
            var wholeWidth = layoutData.frameWidth;
            var pointerWidth = wholeWidth * params.visiblePart;                         // + 2 * pointerOffsetAgainstFrame);
            var pointerLeft = wholeWidth * params.leftOffset + layoutData.frameLeft;    // - pointerOffsetAgainstFrame);

            $(pointerContainer).width(pointerWidth);
            $(pointerContainer).css('left', pointerLeft + 'px');

        }

        (function bindEvents() {
            $(pointerInside).bind({
                mousedown: function (e) {
                    turnOnMoveMode(e);
                },
                mouseup: function (e) {
                    turnOffMoveMode();
                }
            });

            $(pointerLeftExpander).bind({
                mousedown: function (e) {
                    turnOnLeftExpandMode();
                },
                mouseup: function (e) {
                    turnOffLeftExpandMode();
                }
            });

            $(pointerRightExpander).bind({
                mousedown: function (e) {
                    turnOnRightExpandMode();
                },
                mouseup: function (e) {
                    turnOffRightExpandMode();
                }
            });

            $(frame).bind({
                mousemove: function (e) {
                    handleMousemove(e);
                }
            });

            $(pointerContainer).bind({
                mousemove: function (e) {
                    handleMousemove(e);
                }
            });

            $(document).bind({
                mouseup: function (e) {
                    turnOffMoveMode();
                    turnOffLeftExpandMode();
                    turnOffRightExpandMode();
                }
            });

        })();



        //Moving & resizing.
        function turnOnMoveMode(e) {
            moveMode = true;
            rightResizeMode = false;
            leftResizeMode = false;
        }

        function turnOffMoveMode() {
            moveMode = false;
        }

        function turnOnLeftExpandMode() {
            moveMode = false;
            rightResizeMode = false;
            leftResizeMode = true;
        }

        function turnOffLeftExpandMode() {
            leftResizeMode = false;
        }

        function turnOnRightExpandMode() {
            moveMode = false;
            rightResizeMode = true;
            leftResizeMode = false;
        }

        function turnOffRightExpandMode() {
            rightResizeMode = false;
        }

        function handleMousemove(e) {
            if (moveMode) {
                move(e);
            } else if (rightResizeMode) {
                resizeToRight(e);
            } else if (leftResizeMode) {
                resizeToLeft(e);
            }
            cursorX = e.pageX;
        }



        function move(e) {
            var offset = e.pageX - cursorX;
            var elementCurrentLeft = pointerContainer.offsetLeft;
            var postLeft = elementCurrentLeft + offset;
            var postRight = postLeft + $(pointerContainer).width();
            var exceedRightBorder = postRight > (frame.offsetWidth + frame.offsetLeft);
            var exceedLeftBorder = postLeft < 0;

            if (!exceedLeftBorder && !exceedRightBorder) {
                $(pointerContainer).css('left', postLeft + 'px');
                triggerPostResizeEvent(false, true);
            }
        }

        function resizeToLeft(e) {
            var offset = e.pageX - cursorX;
            var elementCurrentLeft = pointerContainer.offsetLeft;
            var elementCurrentWidth = pointerContainer.offsetWidth;
            var postLeft = elementCurrentLeft + offset;
            var postWidth = elementCurrentWidth - offset
            var exceedLeftBorder = postLeft < 0;

            if (!exceedLeftBorder && postWidth >= minWidth) {
                var postWidth = elementCurrentWidth - offset;
                $(pointerContainer).width(postWidth);
                $(pointerContainer).css('left', postLeft + 'px');
                triggerPostResizeEvent(true, false);
            }
        }

        function resizeToRight(e) {
            var offset = e.pageX - cursorX;
            var elementCurrentWidth = pointerContainer.offsetWidth;
            var postWidth = elementCurrentWidth + offset
            var postRight = pointerContainer.offsetLeft + postWidth;
            var exceedRightBorder = postRight > (frame.offsetWidth + frame.offsetLeft);

            if (!exceedRightBorder && postWidth >= minWidth) {
                $(pointerContainer).width(postWidth);
                triggerPostResizeEvent(true, false);
            }
        }

        function triggerPostResizeEvent(resized, relocated) {
            var containerLeft = pointerContainer.offsetLeft - frame.offsetLeft;
            var frameWidth = frame.offsetWidth;
            params = {
                relativeLeft: (containerLeft / frameWidth),
                relativeWidth: (pointerContainer.offsetWidth / frameWidth),
                resized: resized,
                relocated: relocated
            }
            self.trigger({
                type: 'postResize',
                params: params
            });
        }


        return {
            frame: frame,
            pointerContainer: pointerContainer,
            pointerInside: pointerInside,
            pointerLeftExpander: pointerLeftExpander,
            pointerRightExpander: pointerRightExpander,
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