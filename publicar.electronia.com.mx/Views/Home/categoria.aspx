<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    categoria
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Flujo de publicación</h1>
            </hgroup><br />
            <h2>Selecciona la categoria de tu artículo</h2>
            
           
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div style="display:inline-block;">
            <ul>
            <% foreach (var cat in ViewBag.Categories)
                { %>

                    <li><a href="javascript:category('<%: cat.category_id%>', 0);"> <%: cat.category_name %> </a></li>
            <% }%>
            </ul>
        </div>
        <div id="child_container" style="display:inline-block;"></div>
         <div id="button_continuar" style="display:inline-block;"></div>
        
    </div>

    <div><a href="javascript:creaElementos(0);">inicio</a></div>
    <div id="hijo_conteiner"></div>
   

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">

    <script>
        var numElement = 0;
        function creaElementos(numdiv) {
            // verificamos si n>numElementos
            if (numElement > numdiv) {
                alert(numElement + "-" + numdiv);
                for (i = 0; numElement > numdiv; i++) {
                    alert("borrando el div" + numElement);
                    var div_id = "#hijo_" + numElement;
                    $(div_id).remove();
                    numElement = numElement - 1;

                }

            }

            numdiv = numdiv + 1;
            alert("vamos a crear el div" + numdiv);
            var items = [];
            var aux = "<div id='hijo_" + numdiv + "' style='display:inline-block;'><li><a href='javascript:creaElementos(" + numdiv + ");'>" + numdiv + "-n=" + numElementos + " </a></li></div>";
            items.push(aux);
            numElement = numElement + 1;
            $('#hijo_conteiner').append(items.join(''));
        }
    </script>
    <script>
        var numElementos = 0;
        function category(id, numdiv) {
            //alert('le diste click con el id=' + id);
            if (numElementos > numdiv) {
                //alert(numElementos + "-" + numdiv);
                for (i = 0; numElementos > numdiv; i++) {
                    //alert("borrando el div" + numElementos);
                    var div_id = "#child_" + numElementos;
                    $(div_id).remove();
                    numElementos = numElementos - 1;

                }

            }


            var API = "/home/categoriaAjax/"+ id;
            $.getJSON(API, function (data) {
                var items = [];
                numdiv = numdiv + 1;
                //alert ('entrando a la api'+API + 'el id='+id);

                //alert('hola'+data.length);	//si datalenght es !=0 si es 0 entonces boton
                if (data.length != 0) {
                    //var i = 0;
                    var category_id = "";
                    var category_name = "";
                    var bandera = 0;
                    for (i = 0; i < data.length; i++) {
                        $.each(data[i], function (key, val) {

                            if (key == "category_id") {
                                category_id = val;
                            }
                            if (key == "category_name") {
                                category_name = val;
                                bandera = 1;
                            }
                            if (bandera == 1) {
                                var aux = "<li><a href='javascript:category(\"" + category_id + "\"," + numdiv + ");'>" + val + " </a></li>";
                                items.push(aux);
                                bandera = 0;
                            }


                        });
                    }


                    var menu = "<div id='child_" + numdiv + "' style='display:inline-block;'><ul>" + items.join('') + "</ul></div>";
                    numElementos = numElementos + 1;
                    $('#child_container').append(menu);
                    $('#button_continuar').empty();
                } else {
                    $('#button_continuar').empty();
                    $('#button_continuar').append('<a href="/home/datosArticulo/'+id+'">Continuar</a>');
                }
                
               
            });


           // var contenido = "<h2>Hola aqui va la categoria hijo</h2>";
            
        }
    </script>
</asp:Content>
