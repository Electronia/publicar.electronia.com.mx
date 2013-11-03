<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Fotos
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Flujo de publicación</h1>
            </hgroup><br />
            <h2>Coloca las fotos de tu articulo con id <%: ViewBag.Message %></h2>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <form id="fileupload" action="/home/process_picture/<%: ViewBag.Message %>" method="POST" enctype="multipart/form-data">
              
                   <div class="row fileupload-buttonbar">
                       <div class="span7">
                            <!-- The fileinput-button span is used to style the file input field as button -->
                           <span class="btn btn-success fileinput-button">
                               <i class="icon-plus icon-white"></i>
                               <span>Seleccionar Fotos</span>
                               <input type="file" name="files[]" multiple>
                           </span>
                           <button type="submit" class="btn btn-primary start">
                               <i class="icon-upload icon-white"></i>
                               <span>Iniciar carga</span>
                           </button>

                           <button type="button" class="btn btn-danger delete">
                               <i class="icon-trash icon-white"></i>
                               <span>Eliminar</span>
                           </button>
                           <input type="checkbox" class="toggle">
                       </div>
                       <div class="span5">
                            <!-- The global progress bar -->
                           <div class="progress progress-success progress-striped active fade">
                               <div class="bar" style="width:0%;"></div>
                           </div>
                       </div>
                   </div>
                   <!-- The loading indicator is shown during file processing -->
                   <div class="fileupload-loading"></div>
                   <br>
                   <!-- The table listing the files available for upload/download -->
                   <table class="table table-striped"><tbody class="files" data-toggle="modal-gallery" data-target="#modal-gallery"></tbody></table>
               </form>
               <br>

            
    <!-- modal-gallery is the modal dialog used for the image gallery -->
           <div id="modal-gallery" class="modal modal-gallery hide fade" data-filter=":odd">
               <div class="modal-header">
                   <a class="close" data-dismiss="modal">&times;</a>
                   <h3 class="modal-title"></h3>
               </div>
               <div class="modal-body"><div class="modal-image"></div></div>
               <div class="modal-footer">
                   <a class="btn modal-download" target="_blank">
                       <i class="icon-download"></i>
                       <span>Descargar</span>
                   </a>
                   <a class="btn btn-success modal-play modal-slideshow" data-slideshow="5000">
                       <i class="icon-play icon-white"></i>
                       <span>Presentación</span>
                   </a>
                   <a class="btn btn-info modal-prev">
                       <i class="icon-arrow-left icon-white"></i>
                       <span>Previa</span>
                   </a>
                   <a class="btn btn-primary modal-next">
                       <span>Siguiente</span>
                       <i class="icon-arrow-right icon-white"></i>
                   </a>
               </div>
           </div>
         



</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
<!-- Bootstrap CSS Toolkit styles -->
<link rel="stylesheet" href="/Content/themes/bostrap/bostrap1.css">
<!-- Generic page styles -->
<link rel="stylesheet" href="/Content/themes/css/style.css">
<!-- Bootstrap styles for responsive website layout, supporting different screen sizes -->
<link rel="stylesheet" href="/Content/themes/bostrap/responsive1.css">
<!-- Bootstrap CSS fixes for IE6 -->
<!--[if lt IE 7]><link rel="stylesheet" href="http://blueimp.github.com/cdn/css/bootstrap-ie6.min.css"><![endif]-->
<!-- Bootstrap Image Gallery styles -->
<link rel="stylesheet" href="/Content/themes/bostrap/gallery1.css">
<!-- CSS to style the file input field as button and adjust the Bootstrap progress bars -->
<link rel="stylesheet" href="/Content/themes/css/jquery.fileupload-ui.css">
<!-- Shim to make HTML5 elements usable in older Internet Explorer versions -->
<!--[if lt IE 9]><script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script><![endif]-->
<!-- The template to display files available for upload -->
            <script id="template-upload" type="text/x-tmpl">
            {% for (var i=0, file; file=o.files[i]; i++) { %}
                <tr class="template-upload fade">
                    <td class="preview"><span class="fade"></span></td>
                    <td class="name"><span>{%=file.name%}</span></td>
                    <td class="size"><span>{%=o.formatFileSize(file.size)%}</span></td>
                    {% if (file.error) { %}
                        <td class="error" colspan="2"><span class="label label-important">{%=locale.fileupload.error%}</span> {%=locale.fileupload.errors[file.error] || file.error%}</td>
                    {% } else if (o.files.valid && !i) { %}
                        <td>
                            <div class="progress progress-success progress-striped active"><div class="bar" style="width:0%;"></div></div>
                        </td>
                        <td class="start">{% if (!o.options.autoUpload) { %}
                            <button class="btn btn-primary">
                                <i class="icon-upload icon-white"></i>
                                <span>{%=locale.fileupload.start%}</span>
                            </button>
                        {% } %}</td>
                    {% } else { %}
                        <td colspan="2"></td>
                    {% } %}
                    <td class="cancel">{% if (!i) { %}
                        <button class="btn btn-warning">
                            <i class="icon-ban-circle icon-white"></i>
                            <span>{%=locale.fileupload.cancel%}</span>
                        </button>
                    {% } %}</td>
                </tr>
            {% } %}
            </script>
            <!-- The template to display files available for download -->
            <script id="template-download" type="text/x-tmpl">
            {% for (var i=0, file; file=o.files[i]; i++) { %}
                <tr class="template-download fade">
                    {% if (file.error) { %}
                        <td></td>
                        <td class="name"><span>{%=file.name%}</span></td>
                        <td class="size"><span>{%=o.formatFileSize(file.size)%}</span></td>
                        <td class="error" colspan="2"><span class="label label-important">{%=locale.fileupload.error%}</span> {%=locale.fileupload.errors[file.error] || file.error%}</td>
                    {% } else { %}
                        <td class="preview">{% if (file.thumbnail_url) { %}
                            <a href="{%=file.url%}" title="{%=file.name%}" rel="gallery" download="{%=file.name%}"><img src="{%=file.thumbnail_url%}"></a>
                        {% } %}</td>
                        <td class="name">
                            <a href="{%=file.url%}" title="{%=file.name%}" rel="{%=file.thumbnail_url&&'gallery'%}" download="{%=file.name%}">{%=file.name%}</a>
                        </td>
                        <td class="size"><span>{%=o.formatFileSize(file.size)%}</span></td>
                        <td colspan="2"></td>
                    {% } %}
                    <td class="delete">
                        <button class="btn btn-danger"  data-type="{%=file.delete_type%}" data-url="{%=file.delete_url%}">
                            <i class="icon-trash icon-white"></i>
                            <span>{%=locale.fileupload.destroy%}</span>
                        </button>
                        <input type="checkbox" name="delete" value="1">
                    </td>
                </tr>
            {% } %}
            </script>
            <script src="/Content/themes/js/jquery1.7.2.js"></script>
            <!-- The jQuery UI widget factory, can be omitted if jQuery UI is already included -->
            <script src="/Content/themes/js/vendor/jquery.ui.widget.js"></script>
            <!--<script src="http://blueimp.github.com/JavaScript-Templates/tmpl.min.js"></script>-->
            <script src="/Content/themes/js/tmp1.min.js"></script>
            <!--<script src="js/tmpl.js"></script>-->
            <!-- The Load Image plugin is included for the preview images and image resizing functionality -->
            <!--<script src="http://blueimp.github.com/JavaScript-Load-Image/load-image.min.js"></script>-->
            <script src="/Content/themes/js/load-image-min.js"></script>
            <!-- The Canvas to Blob plugin is included for image resizing functionality -->
            <!--<script src="http://blueimp.github.com/JavaScript-Canvas-to-Blob/canvas-to-blob.min.js"></script>-->
            <script src="/Content/themes/js/canvas-to-blob-min.js"></script>
            <!-- The Templates plugin is included to render the upload/download listings -->

            <!-- The Load Image plugin is included for the preview images and image resizing functionality -->
            <!--<script src="js/load-imagen-min.js"></script>-->
            <!-- The Canvas to Blob plugin is included for image resizing functionality -->
            <!--<script src="js/canvas-to-blob-min.js"></script>-->
            <!-- Bootstrap JS and Bootstrap Image Gallery are not required, but included for the demo -->
            <script src="/Content/themes/js/bootstrap.js"></script>
            <script src="/Content/themes/js/image-gallery.js"></script>
            <!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
            <script src="/Content/themes/js/jquery.iframe-transport.js"></script>
            <!-- The basic File Upload plugin -->
            <script src="/Content/themes/js/jquery.fileupload.js"></script>
            <!-- The File Upload file processing plugin -->
            <script src="/Content/themes/js/jquery.fileupload-fp.js"></script>
            <!-- The File Upload user interface plugin -->
            <script src="/Content/themes/js/jquery.fileupload-ui.js"></script>
            <!-- The localization script -->
            <script src="/Content/themes/js/locale.js"></script>
            <!-- The main application script -->
            <script src="/Content/themes/js/main.js"></script>
            <!-- The XDomainRequest Transport is included for cross-domain file deletion for IE8+ -->
            <!--[if gte IE 8]><script src="js/cors/jquery.xdr-transport.js"></script><![endif]-->

</asp:Content>
