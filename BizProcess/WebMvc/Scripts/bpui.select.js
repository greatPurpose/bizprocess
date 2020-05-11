//下拉选择
; BPUI.Select = function ()
{
    var instance = this;
    this.init = function ($selects)
    {
        initElement($selects, "select");
    };
}