<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    datosArticulo
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
     <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Flujo de publicación</h1>
            </hgroup><br />
            <h2>Coloca los datos de tu articulo de la categoria <%: ViewBag.Message %></h2>
            
           
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form name="dataItem" action="/home/fotos/" method="post">
    <input type="hidden" name="category_id" value="<%: ViewBag.Message %>" />
    <div>
        <div style="display:inline-block;">Titulo:</div>
        <div style="display:inline-block;"><input name="item_title" type="text" size="80" /></div>
    </div>
     <div>
        <div style="display:inline-block;">Precio</div>
         <div style="display:inline-block;"><input name="item_price" type="text" size="10" /></div>
         <div style="display:inline-block;">
                <select name="item_typeCurrency">
                    <option value="MXN">MXN</option>
                    <option value="USD">USD</option>
                </select>

         </div>
         <div style="display:inline-block;"><input type="radio" name="item_type_seller" value="credit" />Crédito <input type="radio" name="item_type_seller" value="contado" checked />Contado </div>
    </div>
     <div>
        <div style="display:inline-block;">Condicion</div>
         <div style="display:inline-block;">
             <input type="radio" name="item_condition" value="nuevo" />Nuevo <input type="radio" name="item_condition" value="usado" checked />Usado 
         </div>
    </div>
     <div>
        <div style="display:inline-block; text-align:start;">Descripción</div>
         <div style="display:inline-block;">
             <textArea cols="40" rows="10" name="description" ></textArea>
         </div>
    </div>
     <div>
        <div style="display:inline-block;"><input type="submit" value="Continuar" /></div>
    </div>
    </form>

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>
