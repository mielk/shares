module('mielk.dates tests', {
    setup: function () {
        $.mockjax.clear();
    },
    teardown: function () {
        $.mockjax.clear();
    }
});


test('Test', function () {
    expect(1);

    //Arrange
    var x = 1;
    
    //Act
    var y = 1;

    //Assert
    equal(x, y, 'X must be equal to Y');

});
