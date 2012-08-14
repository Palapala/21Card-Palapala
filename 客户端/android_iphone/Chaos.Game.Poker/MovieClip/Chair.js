/*
    椅子
*/
var Chair = function (props) {
    this.seat = props.seat;

    Chair.superClass.constructor.call(this, props);
    this.id = props.id || Quark.UIDUtil.createUID("Chair");
};
Quark.inherit(Chair, Quark.Button);
