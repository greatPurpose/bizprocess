﻿var wf_r=null,appid=null,groupid=null,wf_steps=[],wf_texts=[],wf_conns=[],wf_option="",wf_focusObj=null,wf_width=108,wf_height=50,wf_rect=8,wf_nodeBorderColor=isshowDesign?"#23508e":"#7a7a7a",wf_noteColor="#efeff0",wf_nodeBorderColor1="green",wf_noteColor1="#e7f0c7",wf_stepDefaultName="",tempArrPath=[],wf_json={},wf_id="",links_tables_fields=[];

$(function(){
    wf_r=Raphael("flowdiv",$(window).width()-20,$(window).height()-65);
    wf_r.customAttributes.type1=function(){};
    wf_r.customAttributes.fromid=function(){};
    wf_r.customAttributes.toid=function(){}
});
function addStep(a, c, b, d, e) {
    var f = getGuid(), g = getNewXY();
    a = a || g.x;
    c = c || g.y;
    b = b || wf_stepDefaultName;
    d = d || f;

    var f = wf_r.rect(a, c, wf_width, wf_height, wf_rect), g = wf_noteColor, h = wf_nodeBorderColor;
    e && (g = wf_noteColor1, h = wf_nodeBorderColor1);
    f.attr({
        fill: g,
        stroke: h,
        "stroke-width": 1.4,
        cursor: "pointer"
    });
    f.id = d;
    f.type1 = "step";
    f.click(function () {
        tryOpenForm(this.id);
    });
    wf_steps.push(f);
    e = 8 < b.length ? b.substr(0, 7) + "..." : b;
    a = wf_r.text(a + 52, c + 25, e);
    a.attr({ "font-size": "12px", cursor: "pointer" });
    8 < b.length && a.attr({ title: b });
    a.id = "text_" + d;
    a.type1 = "text";
    a.click(function () {
        tryOpenForm(this.id.replace('text_', ''));
    });
    wf_texts.push(a)
}
function addDivFlow(a, c, b, d, e) {
    var f=getGuid(),g=getNewXY();
    a=a||g.x;
    c=c||g.y;
    b=b||wf_stepDefaultName;
    d = d || f;
 
    var f=wf_r.rect(a,c,wf_width,wf_height,wf_rect),g=wf_noteColor,h=wf_nodeBorderColor;
    e&&(g=wf_noteColor1,h=wf_nodeBorderColor1);
    f.attr({
        fill:g,
        stroke:h,
        "stroke-width":1.4,
        cursor: "pointer"
    });
    f.id=d;
    f.type1 = "step";
    f.click(function () {
        tryOpenForm(this.id);
    });
    wf_steps.push(f);
    e=8<b.length?b.substr(0,7)+"...":b;
    a=wf_r.text(a+52,c+25,e);
    a.attr({"font-size":"12px", cursor: "pointer"});
    8<b.length&&a.attr({title:b});
    a.id="text_"+d;
    a.type1="text";
    a.click(function () {
        tryOpenForm(this.id.replace('text_', ''));
    });
    wf_texts.push(a)
}

function tryOpenForm(stepid) {
    $.ajax({
        url: "../WorkFlowTasks/GetTaskStepDetails?flowid=" + wf_id + "&stepid=" + stepid + "&groupid=" + groupid,
        async: !1,
        cache: !1,
        dataType: "json",
        success: function (a) {
            url = "WorkFlowRun/Index?flowid=" + a.flowid + "&stepid=" + a.stepid + "&instanceid=" + a.instanceid + "&taskid=" + a.taskid + "&groupid=" + a.groupid + "&appid=" + appid + "&display=1";
            title = a.stepname;
            top.mainDialog.open({
                url: top.rootdir + url,
                width: screen.width - 50, height: screen.height - 100, title: title
            });
        }
    })

}

function setStepText(a,c){
    var b=wf_r.getById("text_"+a);
    null != b && (8 < c.length && (b.attr({ title: c }), c = c.substr(0, 7) + "..."), b.attr({ text: c }))
}

function setLineText(a,c){
    for(var b,d=0;d<wf_conns.length;d++)
        if(wf_conns[d].id==a){
            b=wf_conns[d];break
        }
    if(b){
        d=b.arrPath.getBBox();
        b=(d.x+d.x2)/2;
        var d=(d.y+d.y2)/2,e=wf_r.getById("line_"+a);
        null!=e?c?(e.attr("x",b),e.attr("y",d),e.attr("text",c||"")):e.remove():c&&(b=wf_r.text(b,d,c),b.type1="line",b.id="line_"+a)
    }
}

function connObj(a,c,b){
    isLine(a)&&(wf_conns.push(wf_r.drawArr(a,c)),setLineText(a.id,b))
}

function isLine(a){
    if(!(a&&a.obj1&&a.obj2&&a.obj1!==a.obj2&&isStepObj(a.obj1)&&isStepObj(a.obj2)))return!1;
    for(var c=0;c<wf_conns.length;c++)
        if(a.obj1===a.obj2||wf_conns[c].obj1===a.obj1&&wf_conns[c].obj2===a.obj2)return!1;
    return!0
}

function isStepObj(a){
    return a&&a.type1&&"step"==a.type1.toString()
}
	
function dragger(){
    this.ox=this.attr("x");
    this.oy=this.attr("y");
    changeStyle(this)
}
	
Raphael.fn.drawArr=function(a,c){
    if(a&&a.obj1){
        if(a.obj2){
            var b=getStartEnd(a.obj1,a.obj2),b=getArr(b.start.x,b.start.y,b.end.x,b.end.y,7);
            try{
                a.arrPath?a.arrPath.attr({path:b}):(a.arrPath=this.path(b),a.arrPath.attr({"stroke-width":1.7,stroke:c?"green":"#898a89",fill:"#898a89"}))
            }catch(d){
            }
            return a
        }
        for(var b=getStartEnd(a.obj1,a.obj2),b=getArr(b.start.x,b.start.y,0,0,7),e=0;e<tempArrPath.length;e++)
            tempArrPath[e].arrPath.remove();
        tempArrPath=[];
        a.arrPath=this.path(b);
        a.arrPath.attr({
            "stroke-width":1.7,
            stroke:c?"green":"#898a89",
            fill:"#898a89"
        });
        tempArrPath.push(a)
    }
};

function getStartEnd(a,c){
    for(var b=a?a.getBBox():null,d=c?c.getBBox():null,b=[{x:b.x+b.width/2,y:b.y-1},{x:b.x+b.width/2,y:b.y+b.height+1},{x:b.x-1,y:b.y+b.height/2},{x:b.x+b.width+1,y:b.y+b.height/2},d?{x:d.x+d.width/2,y:d.y-1}:{},d?{x:d.x+d.width/2,y:d.y+d.height+1}:{},d?{x:d.x-1,y:d.y+d.height/2}:{},d?{x:d.x+d.width+1,y:d.y+d.height/2}:{}],d={},e=[],f=0;4>f;f++)
        for(var g=4;8>g;g++){
            var h=Math.abs(b[f].x-b[g].x),k=Math.abs(b[f].y-b[g].y);
            if(f==g-4||(3!=f&&6!=g||b[f].x<b[g].x)&&(2!=f&&7!=g||b[f].x>b[g].x)&&(0!=f&&5!=g||b[f].y>b[g].y)&&(1!=f&&4!=g||b[f].y<b[g].y))e.push(h+k),d[e[e.length-1]]=[f,g]
        }
    d=0==e.length?[0,4]:d[Math.min.apply(Math,e)];
    e={start:{},end:{}};
    e.start.x=b[d[0]].x;
    e.start.y=b[d[0]].y;
    e.end.x=b[d[1]].x;
    e.end.y=b[d[1]].y;
    return e
}

function getArr(a,c,b,d,e){
    var f=Raphael.angle(a,c,b,d),g=Raphael.rad(f-28),h=Raphael.rad(f+28),f=b+Math.cos(g)*e,g=d+Math.sin(g)*e,k=b+Math.cos(h)*e;
    e=d+Math.sin(h)*e;
    return["M",a,c,"L",b,d,"M",b,d,"L",k,e,"L",f,g,"z"].join()
}
	
function getNewXY(){
    var a=10,c=50;0<wf_steps.length&&(c=wf_steps[wf_steps.length-1],a=parseInt(c.attr("x"))+170,c=parseInt(c.attr("y")),a>wf_r.width-wf_width&&(a=10,c+=100),c>wf_r.height-wf_height&&(c=wf_r.height-wf_height));
    return{x:a,y:c}
}

function getGuid(){
    return Raphael.createUUID().toLowerCase()
}
	
function initwf(){
    wf_json={};
    wf_steps=[];
    wf_texts=[];
    wf_conns=[];
    wf_r.clear()
}
	
Array.prototype.remove=function(a){
    if(isNaN(a)||a>this.length)return!1;
    this.splice(a,1)
};
	
function removeArray(a,c){
    if(isNaN(c)||c>a.length)return!1;
    a.splice(c,1)
}
	
function reloadFlow(a){
    if(!a||!a.id||""==$.trim(a.id))return!1;
    wf_json=a;
    wf_id=wf_json.id;
    wf_r.clear();
    wf_steps=[];
    wf_conns=[];
    wf_texts=[];
    var c=wf_json.steps;
    if(c&&0<c.length){
        for(a=0;a<c.length;a++){
            for(var b=!1,d=0;d<taskJSON.length;d++)
                if(taskJSON[d].stepid.toLowerCase()==c[a].id.toLowerCase()){
                    b=!0;
                    break
                }
            addStep(c[a].position.x,c[a].position.y,c[a].name,c[a].id,b)
        }
        for(a=0;a<taskJSON.length;a++)("0"==taskJSON[a].status||"1"==taskJSON[a].status)&&(b=wf_r.getById(taskJSON[a].stepid))&&b.attr({fill:"#fdddb3",stroke:"#fd6703"})
    }
    if((c=wf_json.lines)&&0<c.length)
        for(a=0;a<c.length;a++){
            b=!1;
            for(d=0;d<taskJSON.length;d++)
                if(taskJSON[d].prevstepid.toLowerCase()==c[a].from.toLowerCase()&&taskJSON[d].stepid.toLowerCase()==c[a].to.toLowerCase()){
                    b=!0;
                    break
                }
            connObj({id:c[a].id,obj1:wf_r.getById(c[a].from),obj2:wf_r.getById(c[a].to)},b,c[a].text)
        }
}

function openFlow1(a, aid, gid){
    appid = aid;
    groupid = gid;

    $.ajax({
        url:"../WorkFlowDesigner/GetJSON?type\x3d0\x26appid\x3d\x26flowid\x3d"+a,
        async:!1,
        cache:!1,
        dataType:"json",
        success: function (a) {
            reloadFlow(a)
        }
    })
};