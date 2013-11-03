<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Flujo de publicación</h1>
                
            </hgroup><br />
            <h2><%: ViewBag.Message %></h2>
           
        </div>
    </section>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
                 <a href="home/categoria/DTM10012">Inmuebles</a> - <a href="home/categoria/DTM20024">Autos,Motos y Otros</a> - <a href="home/categoria/DTM30036">Servicios</a> -
                <a href="home/categoria/DTM40048">Productos</a>
     </div>
</asp:Content>
