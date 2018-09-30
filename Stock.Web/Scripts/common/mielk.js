/// <reference path="tree.js" />
/// <reference path="tree.js" />
/// <reference path="tree.js" />
/*
 * mielk JavaScript library v0.0.1
 *
 * Date: 2014-02-21 14:31
 *
 */
(function(window) {

    'use strict';

    //Classes

    function HashTable(obj) {
        var self = this;
        self.HashTable = true;
        self.length = 0;
        self.items = {};
        

        (function init() {
            
            if (obj instanceof Object) {
                for (var key in obj) {
                    if (obj.hasOwnProperty(key)) {
                        self.items[key] = obj[key];
                        self.length++;
                    }
                }
            }
            
        })();

        this.setItem = function (key, value) {
            var previous;
            if (this.hasItem(key)) {
                previous = this.items[key];
            } else {
                this.length++;
            }
            this.items[key] = value;
            return previous;
        };

        this.getItem = function (key) {
            return this.hasItem(key) ? this.items[key] : undefined;
        };

        this.hasItem = function (key) {
            return this.items.hasOwnProperty(key);
        };

        this.removeItem = function (key) {
            if (this.hasItem(key)) {
                var previous = this.items[key];
                this.length--;
                delete this.items[key];
                return previous;
            } else {
                return undefined;
            }
        };

        this.keys = function () {
            var keys = [];
            for (var k in this.items) {
                if (this.hasItem(k)) {
                    keys.push(k);
                }
            }
            return keys;
        };

        this.values = function () {
            var values = [];
            for (var k in this.items) {
                if (this.hasItem(k)) {
                    values.push(this.items[k]);
                }
            }
            return values;
        };

        this.each = function (fn) {
            for (var k in this.items) {
                if (this.hasItem(k)) {
                    fn(k, this.items[k]);
                }
            }
        };

        this.size = function () {
            return this.length;
        };

        this.clear = function () {
            this.items = {};
            this.length = 0;
        };

        this.clone = function() {
            var clone = {};
            for (var key in self.items) {
                if (self.items.hasOwnProperty(key)) {
                    clone[key] = self.items[key];
                }
            }
            
            return clone;
            
        };

    }

    function EventHandler() {
        this.EventHandler = true;
        var listener = {};

        return {
            bind: function(e) {
                $(listener).bind(e);
            },
            trigger: function(e) {
                $(listener).trigger(e);
            }
        };

    }

    function ResizableDiv(params) {
        var self = this;
        self.ResizableDiv = true;
        self.eventHandler = new EventHandler();

        self.id = params.id || 'id';
        self.parent = params.parent;
        self.minHeight = params.minHeight || 200;
        self.maxHeight = params.maxHeight || 800;

        //State.
        self.resizable = false;
        self.isMinimized = false;

        //Current position.
        self.x = 0;
        self.y = 0;

        //GUI.
        self.ui = (function () {
            var menuBarId = '_bar';
            var containerId = '_container';
            var resizerId = '_resizer';

            var container = $('<div>', {
                id: self.id + containerId,
                'class': 'resizable-panel'
            });
            $(container).appendTo(self.parent);

            var menubar = $('<div>', {
                id: self.id + menuBarId,
                html: self.id,
                'class': 'menuBar'
            }).dblclick(function (e) {
                if (self.isMinimized) {
                    self.maximize();
                } else {
                    self.minimize();
                }
            });
            menubar.appendTo(container);

            var div = $('<div>', {
                id: self.id,
                'class': params.class
            });
            div.appendTo(container);
            //Set initial height if applicable.
            if (params.height) {
                $(div).height(params.height);
            }

            var resizer = $('<div>', {
                id: self.id + resizerId,
                'class': 'resizer'
            }).bind({
                mousedown: function (e) {
                    e.preventDefault();
                    self.setAsResizable(true);
                }
            });
            resizer.appendTo(container);


            return {
                container: container,
                content: div,
                height: function () {
                    return $(div).height();
                },
                setHeight: function (height) {
                    $(div).height(height);
                },
                hide: function () {
                    $(div).css({
                        'display': 'none'
                    });
                    $(resizer).css({
                        'display': 'none'
                    });
                },
                show: function () {
                    $(div).css({
                        'display': 'block'
                    });
                    $(resizer).css({
                        'display': 'block'
                    });
                },
                css: function (css) {
                    $(div).css(css);
                },
                getX: function (e) {
                    return (mielk.ui.getPosition(e).x - $(div).offset().left);
                },
                getY: function (e) {
                    return (mielk.ui.getPosition(e).y - $(div).offset().top);
                },
                setCaption: function (caption) {
                    $(menubar).html(caption);
                }
            };

        })();

        //Events binder.
        var events = (function () {

            $(document).bind({
                mousemove: function (e) {
                    e.preventDefault();
                    self.resize(e);
                },
                mouseup: function (e) {
                    self.setAsResizable(false);
                }
            });

        })();

    }
    ResizableDiv.prototype = {
        bind: function(e){
            this.eventHandler.bind(e);
        },
        trigger: function(e){
            this.eventHandler.trigger(e);
        },
        container: function(){
            return this.ui.container;
        },
        content: function () {
            var div = this.ui.content[0];
            return div;
        },
        resize: function (e) {
            if (this.resizable) {
                var prevY = (this.y ? this.y : -1);
                this.y = this.getY(e);
                this.y = (this.y > this.maxHeight ? this.maxHeight : (this.y < this.minHeight ? this.minHeight : this.y));
                if (prevY === -1) prevY = this.y;

                var height = this.y - prevY + this.ui.height();
                height = (height < this.minHeight ? this.minHeight : (height > this.maxHeight ? this.maxHeight : height));
                this.ui.setHeight(height);

                this.eventHandler.trigger({
                    type: 'resize',
                    height: height
                });

            }
        },
        minimize: function () {
            this.ui.hide();
            this.isMinimized = true;
        },
        maximize: function () {
            this.ui.show();
            this.isMinimized = false;
        },
        setAsResizable: function (value) {
            if (value) {
                this.resizable = true;
                this.ui.css('backgroundColor', '#D6EFF9;');
            } else {
                this.resizable = false;
                this.ui.css('backgroundColor', 'white;');
                this.x = 0;
                this.y = 0;
            }
        },
        disableResizing: function () {
            //MOUSE_CLICKED = 0;
            //mainFrame.setCurrentDiv(null);
            this.ui.css({ 'backgroundColor': 'white;' });
            this.resized = false;
            this.x = 0;
            this.y = 0;
        },
        enableResizing: function () {
            this.ui.css({ 'backgroundColor': '#D6EFF9;' });
            this.resized = true;
            this.resize();
        },
        getX: function () {
            return this.ui.getX();
        },
        getY: function () {
            return this.ui.getY();
        },
        setCaption: function (caption) {

        }
    };




    //Modules

    var objects = (function() {

        //Class inheritance.
        function extend(base, sub) {
            // Avoid instantiating the base class just to setup inheritance
            // See https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/create
            // for a polyfill
            sub.prototype = Object.create(base.prototype);
            // Remember the constructor property was set wrong, let's fix it
            sub.prototype.constructor = sub;
            // In ECMAScript5+ (all modern browsers), you can make the constructor property
            // non-enumerable if you define it like this instead
            Object.defineProperty(sub.prototype, 'constructor', {
                enumerable: false,
                value: sub
            });
        }

        function isFunction(object) {
            return (object && typeof (object) === 'function');
        }

        function addProperties(object, properties) {
            for (var key in properties) {
                if (properties.hasOwnProperty(key)) {
                    object[key] = properties[key];
                }
            }
            return object;
        }

        return {
            extend: extend,
            isFunction: isFunction,
            addProperties: addProperties
        };


    })();

    var notify = (function () {
        var options = {};
        var defaultSettings = {
            clickToHide: true,
            autoHide: true,
            autoHideDelay: 2000,
            arrowShow: false,
            elementPosition: 'bottom right',
            globalPosition: 'bottom right',
            style: 'bootstrap',
            className: 'info',
            showAnimation: 'slideDown',
            showDuration: 400,
            hideAnimation: 'slideUp',
            hideDuration: 500,
            gap: 2
        };


        /*
         * reset
         * Function to restore default settings.
         */
        function reset() {
            options = {
                clickToHide: defaultSettings.clickToHide,
                autoHide: defaultSettings.autoHide,
                autoHideDelay: defaultSettings.autoHideDelay,
                arrowShow: defaultSettings.arrowShow,
                elementPosition: defaultSettings.elementPosition,
                globalPosition: defaultSettings.globalPosition,
                style: defaultSettings.style,
                className: defaultSettings.className,
                showAnimation: defaultSettings.showAnimation,
                showDuration: defaultSettings.showDuration,
                hideAnimation: defaultSettings.hideAnimation,
                hideDuration: defaultSettings.hideDuration,
                gap: defaultSettings.gap
            };
        }


        /*
         * display
         * Function to display message box on the screen.
         */
        function display(message, state, properties) {
            changeState(state);
            applyCustomProperties(properties);
            $.notify(message, options);
            reset();
        }


        /*
         * Function to change the class of the displayed
         * notification based on the given string.
         */
        function changeState(state) {
            if (state === true || state === 'success') {
                options.className = 'success';
            } else if (state === false || state === 'error') {
                options.className = 'error';
            } else if (state === 'warn' || state === 'warning') {
                options.className = 'warn';
            } else {
                options.className = defaultSettings.className;
            }
        }



        /*
         * applyCustomProperties
         * Function to apply custom properties for
         * the notification to be displayed.
         */
        function applyCustomProperties(properties) {
            for (var key in properties) {
                if (properties.hasOwnProperty(key)) {
                    options[key] = properties[key];
                }
            }
        }


        /*
         * changeSettings
         */
        function changeSettings(properties) {
            for (var key in properties) {
                if (properties.hasOwnProperty(key)) {
                    defaultSettings[key] = properties[key];
                }
            }
        }


        /*
         * Initializing this function.
         */
        (function initialize() {
            reset();
        })();
        


        return {
            display: display,
            changeSettings: changeSettings
        };



    })();

    var validation = (function() {

        function coalesce(value, ifFalse) {
            return value ? value : ifFalse;
        }


        function isNumber(n) {
            return typeof n === 'number' && !isNaN(parseFloat(n)) && isFinite(n);
        }


        function isString(s) {
            return typeof s === 'string';
        }


        return {
            coalesce: coalesce,
            isNumber: isNumber,
            isString: isString
        };


    })();

    var db = (function () {

        return {
            fetch: function (controller, method, data, params) {
                var $result;
                var callback = (params && params.callback && typeof (params.callback) === 'function' ? params.callback : null);
                var err = (params && params.err && typeof (params.err) === 'function' ? params.err : null);

                $.ajax({
                    url: '/' + controller + '/' + method,
                    type: 'GET',
                    data: data,
                    datatype: 'json',
                    async: (params && params.async ? true : false),
                    cache: false,
                    traditional: (params && params.traditional ? true : false),
                    success: function (result) {
                        if (callback) {
                            $result = callback(result);
                        } else {
                            $result = result;
                        }
                    },
                    error: function (msg) {
                        if (err) {
                            err(msg);
                        } else {
                            alert(msg.status + ' | ' + msg.statusText);
                        }
                    }
                });

                return $result;

            }
        };

    })();

    var ui = (function(){

        function getComponentCssOffset(component, offsetType) {
            var cssValue = $(component).css(offsetType);
            var numericValue = text.onlyDigits(cssValue, true);
            return numericValue[0];
        }

        function getPosition(e) {
            var x, y;

            if (!e) e = window.event;
            if (e.pageX || e.pageY) {
                x = e.pageX;
                y = e.pageY;
            } else if (e.clientX || e.clientY) {
                x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
                y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
            }

            return {
                x: x,
                y: y
            };

        }

        function getPositionInElement(e, element) {
            //var x, y;
            //var imgPos = findPosition(element);

            //if (!e) e = window.event;

            //if (e.pageX || e.pageY) {
            //    x = e.pageX;
            //    y = e.pageY;
            //} else if (e.clientX || e.clientY) {
            //    x = e.clientX + document.body.scrollLeft
            //      + document.documentElement.scrollLeft;
            //    y = e.clientY + document.body.scrollTop
            //      + document.documentElement.scrollTop;
            //}

            //return {
            //    x: x - imgPos[0],
            //    y: y - imgPos[1]
            //};
            
        }

        function findPosition(element) {
            //if (element.offsetParent !== undefined) {
            //    for (var x = 0, y = 0; element; element = element.offsetParent) {
            //        x += element.offsetLeft;
            //        y += element.offsetTop;
            //    }

            //    return {
            //        x: x,
            //        y: y
            //    };

            //} else {
                
            //    return {
            //        x: element.x,
            //        y: element.y
            //    };
                
            //}
        }


        return {
            getPosition: getPosition,
            getPositionInElement: getPositionInElement,
            findPosition: findPosition,
            getComponentCssOffset: getComponentCssOffset
        };

    })();

    var spinner = (function () {
        var background = null;
        var $spinner = null;

        function createBackground() {
            background = jQuery('<div/>').css({
                'background-color': 'transparent',
                'z-index': 9999,
                'position': 'absolute',
                'width': '20%',
                'height': '150px',
                'left': '40%',
                'right': '40%',
                'top': '200px',
                'display': 'none'
            }).appendTo($(document.body));
        }

        function start() {
            if (!background) createBackground();
            $(background).css('display', 'block');
            $spinner = new SpinnerWrapper($(background));
        }

        function stop() {
            $(background).css('display', 'none');
            if ($spinner) $spinner.stop();
        }

        return {
            start: start,
            stop: stop
        };

    })();

    var arrays = (function () {

        function getLastItem(array) {
            var item = array[array.length - 1];
            return item;
        }

        function fromObject(object) {
            var array = [];
            for (var key in object) {
                if (object.hasOwnProperty(key)) {
                    var item = object[key];
                    array.push(item);
                }
            }
            return array;
        }

        function equal(arr1, arr2) {
            if (arr1 && arr2) {
                if (arr1.length === arr2.length) {
                    for (var i = 0; i < arr1.length; i++) {
                        var object = arr1[i];
                        var found = false;
                        for (var j = 0; j < arr2.length; j++) {
                            // ReSharper disable once ExpressionIsAlwaysConst
                            var obj2 = arr2[j];
                            if (!found && obj2 === object) {
                                found = true;
                            }
                        }

                        if (!found) {
                            return false;
                        }

                    }

                    return true;

                }
                return false;
            }

            return false;

        }

        function remove(array, item) {
            var after = [];
            if (!array || !array.length) return array;

            for (var i = 0; i < array.length; i++) {
                var object = array[i];
                if (object !== item) {
                    after.push(object);
                }
            }

            return after;

        }

        function getMax(array, fn, start, end) {
            var result = null;
            var $start = start || 0;
            var $end = Math.min(end, array.length - 1) || array.length - 1;
            for (var i = $start; i < $end; i++) {
                var item = array[i];
                var value = fn(item);
                if (value !== undefined && (!result || value > result)) result = value;
            }

            return result;

        }

        function getMin(array, fn, start, end) {
            var result = null;
            var $start = start || 0;
            var $end = Math.min(end, array.length - 1) || array.length - 1;
            for (var i = $start; i < $end; i++) {
                var item = array[i];
                var value = fn(item);
                if (value !== undefined && (!result || value < result)) result = value;
            }

            return result;

        }
        
        function firstGreater(array, value, fn, returnIndex) {
            
            if (array) {
                var size = array.length;
                var start = 0;
                var end = size - 1;
                
                //If the first item is greater than value searched
                //or the last item is less than value searched, null is returned.
                if (fn(array[start], value) === 1 && fn(array[end], value) === -1) {
                    return null;
                }

                do {

                    var index = Math.round((end - start) / 2) + start;

                    var item = array[index];
                    var result = fn(item, value);
                    if (result === true) {
                        return returnIndex ? index : item;
                    } else if (result === 1) {
                        end = index - 1;
                    } else if (result === -1) {
                        start = index + 1;
                    }

                } while (end >= start);

            }

            return null;

        }

        function findItem(array, value, fn) {
            var size = array.length;
            var start = 0;
            var end = size - 1;
            var index = Math.round((end - start) / 2);

            var item = array[index];
            var x = 1;
        }

        return {
            getLastItem: getLastItem,
            fromObject: fromObject,
            equal: equal,
            remove: remove,
            getMax: getMax,
            getMin: getMin,
            findItem: findItem,
            firstGreater: firstGreater
        };

    })();

    var numbers = (function () {

        function generateUuid() {
            var d = new Date().getTime();
            var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (d + Math.random() * 16) % 16 | 0;
                d = Math.floor(d / 16);
                return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
            });
            return uuid;
        };

        function log10(x) {
            return Math.log(x) / Math.LN10;
        }

        function addThousandSeparator(number, separator) {
            number += '';
            var x = number.split('.');
            var x1 = x[0];
            var x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + separator + '$2');
            }
            return x1 + x2;
        }


        return {
            generateUUID: generateUuid,
            log10: log10,
            addThousandSeparator: addThousandSeparator
        };

    })();

    var text = (function() {

        function onlyDigits(s, includeNumericSigns) {
            if (includeNumericSigns) {
                var regex = /[+-]?\d+(?:\.\d+)?/g;
                return regex.exec(s + '').map(Number);
            } else {
                return (s + '').match(/^-?\d*/g);
            }
        }

        function countMatchedEnd(base, compared) {
            var counter = 0;
            var baseLength = base.length;
            var comparedLength = compared.length;
            for (var i = 1; i < comparedLength; i++) {

                if (i > baseLength) return counter;

                var $base = base.charAt(baseLength - i);
                var $compared = compared.charAt(comparedLength - i);

                if ($base !== $compared) {
                    return counter;
                } else {
                    counter++;
                }

            }

            return counter;

        }

        function substring(base, start, end, isCaseSensitive) {
            var tempBase, tempStart, tempEnd;

            //Checks if all the parameters are defined.
            if (base === undefined || base === null || start === undefined || start === null || end === undefined || end === null) {
                return '';
            }


            if (isCaseSensitive) {
                tempBase = base ? base.toString() : 0;
                tempStart = start ? start.toString() : 0;
                tempEnd = end ? end.toString() : 0;
            } else {
                tempBase = base.toString().toLowerCase();
                tempStart = start.toString().toLowerCase();
                tempEnd = end.toString().toLowerCase();
            }


            //Wyznacza pozycje początkowego i końcowego stringa w stringu bazowym.
            var iStart = (tempStart.length ? tempBase.indexOf(tempStart) : 0);
            //alert('baseString: ' + baseString + '; start: ' + start + '; end: ' + end + '; caseSensitive: ' + isCaseSensitive);
            if (iStart < 0) {
                return '';
            } else {
                var iEnd = (tempEnd.length ? tempBase.indexOf(tempEnd, iStart + tempStart.length) : tempBase.length);
                return (iEnd < 0 ? '' : base.toString().substring(iStart + tempStart.length, iEnd));
            }
            
        }

        function isLetter($char) {
            return ($char.length === 1 && $char.match(/[a-z]/i) ? true : false);
        }

        function containLettersNumbersUnderscore (str) {
            return (str.match(/^\w+$/) ? true : false);
        }

        function isValidMail(mail) {
            return (mail.match(/^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/) ? true : false);
        }

        function startsWith(base, prefix) {
            var s = base.substr(0, prefix.length);
            return (s === prefix);
        }

        function parse(txt) {
            if (txt === '*' || txt === 'true') {
                return true;
            } else if (txt === '' || txt === 'false') {
                return false;
            } else if ($.isNumeric(txt)) {
                return Number(txt);
            } else {
                return txt;
            }
        }

        function valueToText(value) {
            if (value === true) {
                return '*';
            } else if (value === false) {
                return '';
            } else {
                return value;
            }
        }

        function matchEnd(base, compared) {
            var counter = countMatchedEnd(base, compared);
            if (counter === 0) return '';
            return compared.substring(compared.length - counter, counter);

        }

        function cut(base, chars) {
            if (chars > base.length) return base;
            return base.substring(0, base.length - chars);
        }

        return {
            cut: cut,
            countMatchedEnd: countMatchedEnd,
            onlyDigits: onlyDigits,
            substring: substring,
            isLetter: isLetter,
            containLettersNumbersUnderscore: containLettersNumbersUnderscore,
            isValidMail: isValidMail,
            startsWith: startsWith,
            valueToText: valueToText,
            matchEnd: matchEnd,
            parse: parse
        };

    })();


    var dates = (function () {

        function fromCSharpDateTime(date) {
            var miliseconds = parseInt(mielk.text.substring(date, "(", ")"));
            var date = new Date(miliseconds);
            return date;
        }

        function toString(date, withTime) {
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            var day = date.getDate();

            if (withTime) {
                var hours = date.getHours();
                var minutes = date.getMinutes();
                var seconds = date.getSeconds();
                var time = (hours < 10 ? '0' : '') + hours + ':' +
                           (minutes < 10 ? '0' : '') + minutes + ':' + 
                           (seconds < 10 ? '0' : '') + seconds;
            }

            return year + '-' +
                (month < 10 ? '0' : '') + month + '-' +
                (day < 10 ? '0' : '') + day + (withTime ? ' ' + time : '');
        }

        function fromString(s) {
            var year = s.substr(0, 4) * 1;
            var month = s.substr(5, 2) * 1 - 1;
            var day = s.substr(8, 2) * 1;

            var hour = 0;
            var minute = 0;
            var second = 0;
            if (s.length > 10) {
                hour = s.substr(11, 2) * 1;
                minute = s.substr(14, 2) * 1;
                second = s.substr(17, 2) * 1;
            }

            return new Date(year, month, day, hour, minute, second);
        }

        function daysDifference(start, end) {
            var milisInDay = 86400000;
            var startDay = Math.floor(start.getTime() / milisInDay);
            var endDay = Math.floor(end.getTime() / milisInDay);
            return (endDay - startDay);
        }

        function weeksDifference(start, end) {
            var result = Math.floor(daysDifference(start, end) / 7);
            return (end.getDay() < start.getDay() ? result : result);
        }


        function addMinutes(baseDate, minutes) {
            var date = baseDate;
            date.setMinutes(date.getMinutes() + minutes);
            return date;
        }

        function addHours(baseDate, hours) {
            var date = baseDate;
            date.setHours(date.getHours() + hours);
            return date;
        }

        function addDays(baseDate, days) {
            var date = new Date(baseDate);
            date.setDate(date.getDate() + days);
            return date;
        }

        function addWeeks(baseDate, weeks) {
            var date = baseDate;
            date.setDate(date.getDate() + 7);
            return date;
        }

        function addMonths(baseDate, months) {
            var date = baseDate;
            date.setMonth(date.getMonth() + 1);
            return date;
        }

        function getMonthName(date, short) {
            var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var shortMonthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            var monthNumber = date.getMonth();
            return (short ? shortMonthNames : monthNames)[monthNumber];
        }

        return {
            toString: toString,
            fromString: fromString,
            daysDifference: daysDifference,
            weeksDifference: weeksDifference,
            fromCSharpDateTime: fromCSharpDateTime,
            addMinutes: addMinutes,
            addHours: addHours,
            addDays: addDays,
            addWeeks: addWeeks,
            addMonths: addMonths,
            getMonthName: getMonthName
        };
        
    })();


    var svg = (function () {
        var ns = 'http://www.w3.org/2000/svg';

        function createSvg() {
            return document.createElementNS(ns, 'svg');
        }

        function createPath(d) {
            var path = document.createElementNS(ns, 'path');
            path.setAttribute('d', d);
            return path;
        }

        function createRectangle(width, height, x, y, fill) {
            var rect = document.createElementNS(ns, 'rect');
            rect.width.baseVal.value = width;
            rect.height.baseVal.value = height;
            rect.x.baseVal.value = x;
            rect.y.baseVal.value = y;
            rect.style.fill = fill;
            return rect;
        }

        function createCircle(cx, cy, radius, fill, stroke) {
            var circle = document.createElementNS(ns, 'circle');
            circle.setAttribute('cx', cx);
            circle.setAttribute('cy', cy);
            circle.setAttribute('r', radius);
            circle.style.fill = fill;
            circle.style.stroke = stroke;
            return circle;
        }

        return {
            createSvg: createSvg,
            createRectangle: createRectangle,
            createPath: createPath,
            createCircle: createCircle
        };

    })();


    /*
     * Wrapper for functions defined above.
     */
    var mielk = {
        hashTable: function(obj) {
            return new HashTable(obj);
        },
        eventHandler: function() {
            return new EventHandler();
        },
        resizableDiv: function(params){
            return new ResizableDiv(params);
        },
        notify: notify,
        objects: objects,
        validation: validation,
        db: db,
        ui: ui,
        spinner: spinner,
        arrays: arrays,
        numbers: numbers,
        text: text,
        dates: dates,
        svg: svg
    };



    // Expose mielk to the global object
    window.mielk = mielk;


})(window);