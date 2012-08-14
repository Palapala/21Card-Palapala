var container, stage, context, width, height, timer, events, em, loader;

var PokerManager = function () {
    this.players = new Array();
    this.chairs = new Array();
    /*
    左上角：1
    右上角：2
    右下角：3
    左下角：4
    */
    this.chairPositions = new Array({ x: 70, y: 100 },
                                   { x: 820, y: 100 },
                                   { x: 820, y: 380 },
                                   { x: 70, y: 380 });

    PokerManager.superClass.constructor.call(this);

    this.addEventListener("loadResourceComplete", function () {
        this.initCommon();
        this.initGame();
    });

    this.init();
    this.start();
    this.loadResources();
};
Quark.inherit(PokerManager, Quark.EventDispatcher);
PokerManager.prototype.init = function () {
    container = Quark.getDOM("container");
    events = Quark.supportTouch ? ["touchend"] : ["mousedown", "mouseup", "mousemove", "mouseover"];
    width = 1000;
    height = 600;
    fps = 30;
    var canvas = Quark.createDOM("canvas", { width: width, height: height });
    container.appendChild(canvas);
    context = new Quark.CanvasContext({ canvas: canvas });
    stage = new Quark.Stage({ width: width, height: height, context: context });
    em = new Quark.EventManager();
    em.registerStage(stage, events);
    timer = new Quark.Timer(1 / fps);
    timer.addListener(stage);
    timer.addListener(Quark.Tween);
};
PokerManager.prototype.start = function () {
    timer.start();
};
PokerManager.prototype.pause = function () {
    timer.pause();
};
PokerManager.prototype.stop = function () {
    timer.stop();
};
PokerManager.prototype.loadResources = function () {
    var loadingManager = new LoadingManager();

    /*--Image Loader--*/
    loader = new Quark.ImageLoader();
    loader.addEventListener("loaded", function (e) {
        loadingManager.update("正在加载资源文件 ... (" + e.target.getLoadedSize() + "/" + e.target.getTotalSize() + ")");
    });
    loader.addEventListener("complete", function (e) {
        for (var imageItem in e.images) {
            PokerResource[imageItem] = e.images[imageItem].image;
        }
        loadingManager.remove();

        pokerManager.dispatchEvent({ type: "loadResourceComplete", target: this });
    });
    loadingManager.add();
    for (var resourceItem in PokerResource) {
        loader.load(PokerResource[resourceItem]);
    }
    /*--Image Loader--*/
};
PokerManager.prototype.initCommon = function () {
    var host = new host();
    host.connect();
};
PokerManager.prototype.initGame = function () {
    /*init background*/
    var bg = new Quark.Bitmap({
        id: "bg",
        image: PokerResource.bg
    });
    stage.addChild(bg);
    /*init background*/

    /*init table*/
    var table = new Quark.Bitmap({
        id: "table",
        image: PokerResource.table,
        x: 100
    });
    stage.addChild(table);
    /*init table*/
};
PokerManager.prototype.updateUserList = function (data) {
    //clear chairs
    for (var index = 0; index < this.chairs.length; index++) {
        stage.removeChild(this.chairs[index]);
    }
    //clear players
    for (var index = 0; index < this.players.length; index++) {
        stage.removeChild(this.players[index]);
    }

    //create chairs
    for (var index = 0; index < this.chairPositions.length; index++) {
        var x = index * 110;
        var seat = index + 1;
        var position = this.chairPositions[index];
        var chair = new Chair({
            image: PokerResource.chair,
            seat: seat,
            width: 110,
            height: 110,
            up: { rect: [x, 0, 110, 110] },
            over: { rect: [x, 110, 110, 110] },
            x: position.x,
            y: position.y
        });
        chair.addEventListener("mousedown", function (e) {
            command("JOIN", this.seat);
        });
        this.chairs.push(chair);
    }

    //add chairs
    for (var index = 0; index < this.chairs.length; index++) {
        //set chair enable
        for (var dataIndex = 0; dataIndex < data.list.length; dataIndex++) {
            if (data.list[dataIndex].Seat == this.chairs[index].seat) {
                this.chairs[index].setEnabled(false);

                var name = "玩家 " + data.list[dataIndex].Seat;
                if (data.list[dataIndex].IsSelf)
                    name = "自己";
                //create player
                var player = new Player({
                    name: name,
                    image: PokerResource.player,
                    bgImage: PokerResource.player_bg,
                    money: 1000,
                    x: this.chairs[index].x,
                    y: this.chairs[index].y,
                    width:233,
                    height:70,
                    regX:61.5,
                    regY:-20
                });
                this.players.push(player);
            }
        }
        stage.addChild(this.chairs[index]);
    }

    //add players
    for (var index = 0; index < this.players.length; index++) {
        stage.addChild(this.players[index]);
    }
};



var LoadingManager = function () {
    this.loaderText = null;
};
LoadingManager.prototype.add = function () {
    this.loaderText = new Quark.Text({
        id: "loaderText",
        text: "",
        color: "#000",
        textAlign: "center",
        font: "20px verdana"
    });
    stage.addChild(this.loaderText);
};
LoadingManager.prototype.remove = function () {
    stage.removeChild(this.loaderText);
};
LoadingManager.prototype.update = function (text) {
    this.loaderText.text = text;
    this.loaderText.x = (stage.width / 2) - (this.loaderText.width / 2);
    this.loaderText.y = (stage.height / 2) - (this.loaderText.height / 2);
};
