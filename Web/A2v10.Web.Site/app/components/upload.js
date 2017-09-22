﻿/*20170921-7037*/
/* services/upload.js */


(function() {

    var url = require('std:url');
    var http = require('std:http');

    Vue.component("a2-upload", {
        /* TODO:
            1. Accept for images/upload
            2. multiple
        */
        template: `
            <label :class="cssClass" @dragover="dragOver" @dragleave="dragLeave">
                <input type="file" @change="uploadImage" multiple accept="image/*" />
            </label>
        `,
        props: {
            item: Object,
            prop: String,
            base: String,
            newItem: Boolean
        },
        data: function () {
            return {
                hover: false
            };
        },
        computed: {
            cssClass() {
                return 'file-upload' + (this.hover ? ' hover' : '');
            }
        },
        methods: {
            dragOver(ev) {
                this.hover = true;
                ev.preventDefault();
            },
            dragLeave(ev) {
                this.hover = false;
                ev.preventDefault();
            },
            uploadImage(ev) {
                let root = window.$rootUrl;
                let id = this.item[this.prop];
                let imgUrl = url.combine(root, '_image', this.base, this.prop, id);
                var fd = new FormData();
                for (let file of ev.target.files) {
                    fd.append('file', file, file.name);
                }
                http.upload(imgUrl, fd).then((result) => {
                    // may be id or [id,id,id]
                    if (result.status === 'OK') {
                        // TODO: // multiple
                        if (this.newItem) {
                            let p0 = this.item.$parent;
                            for (let id of result.ids) {
                                let ni = p0.$append();
                                ni[this.prop] = id;
                            }
                        } else {
                            this.item[this.prop] = result.ids[0];
                        }
                    }
                    //alert('result =' + JSON.stringify(result));
                });
            }
        }
    });

})();
