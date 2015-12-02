/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	 config.language = 'zh';
    // config.uiColor = '#AADC6E';
	 config.extraPlugins = 'imgur';
    // Get your client-id from https://api.imgur.com/oauth2/addclient
	 config.imgurClientId = '4548ca9d27ed6e6';
     config.contentsCss = ['../Content/css/editor.css'];
    config.toolbar = [
        {
            name: "basicstyles",
            items: ["FontSize", "Bold", "Italic", "-", "JustifyLeft", "JustifyCenter", "JustifyRight", 'Font']
        },
        {
            name: "paragraph",
            items: ["NumberedList", "BulletedList", "-"]
        }, {
            name: "tools",
            items: ["Maximize", "-"]
        }, {
            name: "links",
            items: ["Link", "Unlink"]
        }, {
            name: 'insert',
            items: ['Table', 'Smiley', 'Iframe']
        }, {
            name: "colors",
            items: ["TextColor", "BGColor"]
        }, {
            name: "clipboard",
            items: ["Cut", "Copy", "Paste", "Undo", "Redo", "-", "Source"]
        }];

};
