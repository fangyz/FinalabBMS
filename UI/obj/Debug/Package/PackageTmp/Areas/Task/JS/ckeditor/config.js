/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.toolbar = 'MyToolbar';
    config.resize_enabled = false;

    config.toolbar_MyToolbar =
    [
            { name: 'document', items: ['Source', 'NewPage', 'Preview'] },
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', '-', 'RemoveFormat'] },
        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
            { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'] },
            '/',
            { name: 'styles', items: ['Styles', 'Format'] },
            { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'] },
        {
            name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley']
        },
            { name: 'tools', items: ['Maximize', '-', 'About'] }
    ];
};
