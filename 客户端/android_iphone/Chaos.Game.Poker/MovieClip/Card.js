/*
    �˿���
 */
var Card = function (props) {
    this.data = null

    Card.superClass.constructor.call(this, props);
    this.id = props.id || Quark.UIDUtil.createUID("Card");

    this._skin = new Quark.MovieClip({
        id: "skin",
        image: props.image
    });
    this._skin.stop(); // ֹͣMovieClip�Ĳ���

    this._border = new Quark.Graphics({
        id: "border",
        x: 0,
        y: 0,
        width: this.width,
        height: this.height
    });
    this._border.lineStyle(2, "#555").drawRect(0, 0, this._border.width, this._border.height).endFill().cache();

    this.addChild(this._skin, this._border);

    this.eventChildren = false; // �ܾ���MovieClip�����¼�
    if (props.useHandCursor === undefined)
        this.useHandCursor = true;
    if (props.close)
        this._setCloseState(props.close);
    if (props.open)
        this._setOpenState(props.open);
};
Quark.inherit(Card, Quark.DisplayObjectContainer);

// �˿���״̬
Card.CLOSE = "close";
Card.OPEN = "open";

Card.prototype._setCloseState = function(closeState) {
	closeState.label = Card.CLOSE;
	this._skin.setFrame(closeState);
	this.closeState = closeState;
	return this;
};
Card.prototype._setOpenState = function(openState) {
	openState.label = Card.OPEN;
	this._skin.setFrame(openState);
	this.openState = openState;
	return this;
};

// �ı��˿���״̬ΪClose
Card.prototype.closeCard = function() {
	if (this.closeState)
		this._skin.gotoAndStop(Card.CLOSE);
};
// �ı��˿���״̬ΪOpen
Card.prototype.openCard = function () {
    if (this.openState)
        this._skin.gotoAndStop(Card.OPEN);
};