//新建表单
UE.plugins['reportadd'] = function ()
{
    var me = this, thePlugins = 'reportadd';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/attribute.aspx?new=1',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '新建表单',
                cssRules: "width:400px;height:310px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
};
//表单属性
UE.plugins['reportattribute'] = function ()
{
    var me = this, thePlugins = 'reportattribute';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/attribute.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '设置表单属性',
                cssRules: "width:400px;height:310px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
};
//打开表单
UE.plugins['reportopen'] = function ()
{
    var me = this, thePlugins = 'reportopen';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/open.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '打开表单',
                cssRules: "width:850px;height:400px;"
            });
            dialog.render();
            dialog.open();
        }
    };
};
//保存表单
UE.plugins['reportsave'] = function ()
{
    var me = this, thePlugins = 'reportsave';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/save.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '保存表单',
                cssRules: "width:300px;height:120px;"
            });
            dialog.render();
            dialog.open();
        }
    };
};
//表单另存为
UE.plugins['reportsaveas'] = function ()
{
    var me = this, thePlugins = 'reportsaveas';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/saveas.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '表单另存为',
                cssRules: "width:400px;height:180px;"
            });
            dialog.render();
            dialog.open();
        }
    };
};
//发布表单
UE.plugins['reportcompile'] = function ()
{
    var me = this, thePlugins = 'reportcompile';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/publish.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '发布表单',
                cssRules: "width:300px;height:120px;"
            });
            dialog.render();
            dialog.open();
        }
    };
};
//文本框
UE.plugins['reporttext'] = function ()
{
    var me = this, thePlugins = 'reporttext';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/text.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '文本框',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>文本框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
//文本域
UE.plugins['reporttextarea'] = function ()
{
    var me = this, thePlugins = 'reporttextarea';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/textarea.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '文本域',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>文本域: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
//html编辑器
/*
UE.plugins['reporthtml'] = function ()
{
    var me = this, thePlugins = 'reporthtml';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/html.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: 'HTML编辑器',
                cssRules: "width:500px;height:260px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>HTML编辑器: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
//组织机构选择框
UE.plugins['reportorg'] = function ()
{
    var me = this, thePlugins = 'reportorg';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/org.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '组织机构选择框',
                cssRules: "width:500px;height:280px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>组织机构选择框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
//数据字典选择框
UE.plugins['reportdictionary'] = function ()
{
    var me = this, thePlugins = 'reportdictionary';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/dictionary.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '数据字典选择框',
                cssRules: "width:500px;height:280px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_dict")
        {
            var html = popup.formathtml('<nobr>数据字典选择框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
*/
//日期时间选择
UE.plugins['reportdatetime'] = function ()
{
    var me = this, thePlugins = 'reportdatetime';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/datetime.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '日期时间选择',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>日期时间选择: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
//下拉列表框
UE.plugins['reportselect'] = function ()
{
    var me = this, thePlugins = 'reportselect';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/select.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '下拉列表框',
                cssRules: "width:600px;height:360px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>下拉列表框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
/*
//子表
UE.plugins['reportsubtable'] = function ()
{
    var me = this, thePlugins = 'reportsubtable';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/subtable.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '子表',
                cssRules: "width:700px;height:460px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>子表: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
//Label标签
UE.plugins['reportlabel'] = function ()
{
    var me = this, thePlugins = 'reportlabel';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/label.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: 'Label标签',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>Label标签: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};
*/
//grid数据表格
UE.plugins['reportgrid'] = function ()
{
    var me = this, thePlugins = 'reportgrid';
    me.commands[thePlugins] = {
        execCommand: function ()
        {
            var dialog = new UE.ui.Dialog({
                iframeUrl: this.options.UEDITOR_HOME_URL + 'plugins/dialogs/grid.aspx',
                name: thePlugins + '_' + (new Date().valueOf()),
                editor: this,
                title: '数据表格',
                cssRules: "width:600px;height:300px;",
                buttons: [
				{
				    className: 'edui-okbutton',
				    label: '确定',
				    onclick: function ()
				    {
				        dialog.close(true);
				    }
				},
				{
				    className: 'edui-cancelbutton',
				    label: '取消',
				    onclick: function ()
				    {
				        dialog.close(false);
				    }
				}]
            });
            dialog.render();
            dialog.open();
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function ()
        {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function ()
        {
            if (window.confirm('确认删除该控件吗？'))
            {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt)
    {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var type1 = el.getAttribute('type1');
        if (/input/ig.test(el.tagName) && type1 == "flow_" + thePlugins.replace('report', ''))
        {
            var html = popup.formathtml('<nobr>数据表格: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html)
            {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else
            {
                popup.hide();
            }
        }
    });
};