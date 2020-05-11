//Text文本框
;BPUI.Text = function ()
{
    var instance = this;
    this.init = function ($texts)
    {
        initElement($texts, "text");
    };
}