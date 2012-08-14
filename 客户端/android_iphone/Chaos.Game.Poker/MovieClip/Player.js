/*
    玩家
*/
var Player = function (props) {
    this.name = "";
    this.money = 0;

    Player.superClass.constructor.call(this, props);
    this.id = props.id || Quark.UIDUtil.createUID("Player");

    this.autoSize = true;

    this._playerBcakgroundImg = new Quark.Bitmap({
        id:"playerBackgroundImg",
        image:props.bgImage
    });

    this._playerImg = new Quark.Bitmap({
        id: "playerImg",
        image: props.image,
        x:35,
        y:10,
        width:50,
        height:50
    });

    this._playerName = new Quark.Text({
        id: "playerName",
        text: this.name,
        color: "#000",
        x: 95,
        y: 10,
        width: 110,
        lineWidth:110,
        font: "12px verdana"
    });

    this._money = new Quark.Text({
        id: "money",
        text: "" + this.money,
        color: "#ff0000",
        x: 95,
        y: 40,
        width: 110,
        lineWidth: 110,
        font: "14px verdana bold"
    });

    this._graphics = new Quark.Graphics({
        id: "graphics",
        width: 50,
        height: 50,
        x:35,
        y:10
    });
    this._graphics.lineStyle(2, "#000").drawRect(0, 0, 50, 50).endFill().cache();

    this.addChild(this._playerBcakgroundImg, this._playerImg, this._playerName, this._money, this._graphics);

    this.eventChildren = false;
};
Quark.inherit(Player, Quark.DisplayObjectContainer);