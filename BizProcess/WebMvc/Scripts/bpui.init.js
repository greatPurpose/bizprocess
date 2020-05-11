;$(window).load(function ()
{
    var roadInit = new BPUI.Init();
    roadInit.calendar();
    roadInit.file();
    roadInit.member();
    roadInit.dict();
    roadInit.validate();
    roadInit.select();
    roadInit.combox();
    roadInit.table();
    roadInit.selectIco();
    roadInit.editor();
    roadInit.text();
    roadInit.textarea();
    roadInit.button();
    roadInit.textFocus();
});

;BPUI.Init = function ()
{
    this.validate = function ()
    {
        new BPUI.Validate().bind($("[validate]"));
    };

    this.textFocus = function ()
    {
        var $txt = $('<input type="text" style="height:0; width:0;" />');
        try
        {
            $("body").prepend($txt);
            $txt.get(0).focus();
            $txt.remove();
        } catch (e) { }
    };

    this.text = function ()
    {
        new BPUI.Text().init($(".mytext"));
    };

    this.textarea = function ()
    {
        new BPUI.Textarea().init($(".mytextarea"));
    };

    this.editor = function ()
    {
        new BPUI.Editor().init($(".myeditor"));
    };

    this.calendar = function ()
    {
        new BPUI.Calendar().init($(".mycalendar"));
    };

    this.select = function ()
    {
        new BPUI.Select().init($(".myselect"));
    };

    this.combox = function ()
    {
        new BPUI.Combox().init($(".mycombox"));
    };

    this.button = function ()
    {
        new BPUI.Button().init($(".mybutton"));
    };

    this.file = function ()
    {
        new BPUI.File().init($(".myfile"));
    };

    this.member = function ()
    {
        new BPUI.Member().init($(".mymember"));
    };

    this.dict = function ()
    {
        new BPUI.Dict().init($(".mydict"));
    };

    this.selectIco = function ()
    {
        $(".myico").each(function ()
        {
            new BPUI.SelectIco({ obj: $(this) });
        });

    };

    this.table = function ()
    {
        $(".listtable tbody tr:even td").removeClass().addClass("listtabletrout");
        $(".listtable tbody tr:odd td").removeClass().addClass("listtabletrover");
    };
}