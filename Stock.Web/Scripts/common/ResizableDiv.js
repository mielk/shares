


function ResizableDiv(parent, divId, divClass, MIN_HEIGHT, MAX_HEIGHT) {
    //Constants
    var MENU_BAR_ID = "_bar"
    var CONTAINER_ID = "_container"
    var RESIZER_ID = "_resizer"
    this.MIN_HEIGHT = MIN_HEIGHT;
    this.MAX_HEIGHT = MAX_HEIGHT;

    //Main objects
    var obj = this;
    this.parent = parent;

    //Current position
    this.x = 0;
    this.y = 0;

    //State
    this.resizable = false;
    this.isMinimized = false;


    //Container ...
    this.container = $('<div>', { id: divId + CONTAINER_ID, 'class': 'panel fill' });
    this.container.appendTo(parent);

    //... and its components.
    this.menubar = $('<div>', { id: divId + MENU_BAR_ID, 'class': 'menuBar fill' })
        .dblclick(function (e) {
            if (obj.isMinimized) {
                obj.maximize();
            } else {
                obj.minimize();
            }
        });
    this.menubar.appendTo(this.container);

    this.div = $('<div>', { id: divId, 'class': divClass });
    this.div.appendTo(this.container);

    this.resizer = $('<div>', { id: divId + RESIZER_ID, 'class': 'resizer fill' });
    this.resizer.bind({
        mousedown: function (e) {
            e.preventDefault();
            obj.setAsResizable(true);
        }
    });
    this.resizer.appendTo(this.container);
    $(document).bind({
        mousemove: function (e) { e.preventDefault(); obj.resize(e); },
        mouseup: function (e) { obj.setAsResizable(false); }
    });

    //EventsListener
    this.events = $('<div>', { id: this.parent.name + '_events_listener', 'class': 'eventsListener' });
    this.events.css('display', 'none');

}


/*
 * Function to resize this container.
 * Parameters of resizing are established based on the current
 * coordinates of the screen pointer.
 */
ResizableDiv.prototype.resize = function (e) {
    if (this.resizable) {
        var prevY = (this.y ? this.y : -1);
        this.y = this.getY(e);
        this.y = (this.y > this.MAX_HEIGHT ? this.MAX_HEIGHT : (this.y < this.MIN_HEIGHT ? this.MIN_HEIGHT : this.y));
        if (prevY == -1) prevY = this.y;

        var height = this.y - prevY + this.div.height();
        height = (height < this.MIN_HEIGHT ? this.MIN_HEIGHT : (height > this.MAX_HEIGHT ? this.MAX_HEIGHT : height));
        this.div.height(height);

        this.events.trigger({
            type: 'resize',
            'height': height
        });
    }
}



/*
* Function to minimize this container (only menu bar is visible).
*/
ResizableDiv.prototype.minimize = function () {
    this.div.hide();
    this.resizer.hide();
    this.isMinimized = true;
}

/*
* Function to maximize this container (all the containers are visible).
*/
ResizableDiv.prototype.maximize = function () {
    this.div.show();
    this.resizer.show();
    this.isMinimized = false;
}


ResizableDiv.prototype.setAsResizable = function (value) {
    if (value) {
        this.resizable = true;
        this.div.css('backgroundColor', '#D6EFF9;');
    } else {
        this.resizable = false;
        this.div.css('backgroundColor', 'white;');
        this.x = 0;
        this.y = 0;
    }
}



ResizableDiv.prototype.disableResizing = function () {
    MOUSE_CLICKED = 0;
    mainFrame.setCurrentDiv(null);
    $(this.div).css({ 'backgroundColor': 'white;' });
    this.resized = false;
    this.x = 0;
    this.y = 0;
}
ResizableDiv.prototype.enableResizing = function () {
    MOUSE_CLICKED = 1;
    mainFrame.setCurrentDiv(this);
    $(this.div).css({ 'backgroundColor': '#D6EFF9;' });
    this.resized = true;
    this.resize();
}


/*
* Function to retrieve the X coordinate of the screen pointer
* in relation to this container.
*/
ResizableDiv.prototype.getX = function (e) {
    return (getPosition(e)[0] - $(this.div).offset().left);
}


/*
* Function to retrieve the Y coordinate of the screen pointer
* in relation to this container.
*/
ResizableDiv.prototype.getY = function (e) {
    return (getPosition(e)[1] - $(this.div).offset().top);
}
