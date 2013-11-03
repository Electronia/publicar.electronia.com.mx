<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Preview
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
        <section class="featured">
        <div class="content-wrapper">
            
            <h2><%: ViewBag.Message %></h2>
            <h3><%: ViewBag.Category %></h3>
            <h3><%: ViewBag.Title %></h3>
            <h3><%: ViewBag.Price %></h3>
            <h3><%: ViewBag.Currency %></h3>
            <h3><%: ViewBag.Condition %></h3>
            <h3><%: ViewBag.Description %></h3>
            <img src="<%: ViewBag.FotoMain %>" />

            
           
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

Aqui va el contenido

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>
