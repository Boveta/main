<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Boveta.Search" %>

<!DOCTYPE html>

<html>
<head>
    <title>Boveta</title>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="description" content="" />
    <meta name="keywords" content="" />
    <!--[if lte IE 8]><script src="css/ie/html5shiv.js"></script><![endif]-->
    <script src="js/skel.min.js"></script>
    <script src="js/init.js"></script>
    <noscript>
        <link rel="stylesheet" href="css/skel.css" />
        <link rel="stylesheet" href="css/style.css" />
        <link rel="stylesheet" href="css/style-wide.css" />
        <link rel="stylesheet" href="css/style-noscript.css" />
    </noscript>
    <!--[if lte IE 9]><link rel="stylesheet" href="css/ie/v9.css" /><![endif]-->
    <!--[if lte IE 8]><link rel="stylesheet" href="css/ie/v8.css" /><![endif]-->
    <style type="text/css">
        .auto-style1 {
            width: 200px;
        }

        .ddl
        {
            background-image:url('Images/Arrowhead-Down-01.png');
            background-position:88px;
            background-repeat:no-repeat;
            text-indent: 0.01px; /*In Firefox*/
            /* Set box properties*/
            border: 1px solid #c4c4c4; 
		    height: 30px; 
		    padding: 4px 4px 4px 4px; 
		    border-radius: 4px; 
		    -moz-border-radius: 4px; 
		    -webkit-border-radius: 4px; 
		    box-shadow: 0px 0px 8px #d9d9d9; 
		    -moz-box-shadow: 0px 0px 8px #d9d9d9; 
		    -webkit-box-shadow: 0px 0px 8px #d9d9d9; 
            /* Set text properties*/
            color: black;
		    font-family: 'Source Sans Pro', sans-serif;
		    font-size: 13pt;
		    font-weight: 500 !important;
		    letter-spacing: -0.025em;
		    line-height: 1.75em;
        }
        
        table.center {
            margin-left:auto; 
            margin-right:auto;
        }

    </style>
</head>
<body class="loading">
    <div id="wrapper">
        <div id="bg"></div>
        <div id="overlay"></div>
        <div id="main">

            <!-- Header -->

            <header id="header">
                <h1>Boveta</h1>
                <p>Make a good decision &nbsp;&bull;&nbsp; Value your house</p>
                <br />
                <form id="ValuatorForm" runat="server">
                    <table class="center">
                        <tr>
                            <td class="auto-style1">
                                <asp:Label runat="server" Width="200px">Address</asp:Label></td>
                            <td>
                                <asp:TextBox ID="TBAddress" runat="server" Width="200px">Krukmakargatan 9</asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Postnummer/Ort</td>
                            <td>
                                <asp:TextBox ID="TBZipCode" runat="server" Width="200px">118 51</asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Size (sqm)</td>
                            <td>
                                <asp:TextBox ID="TBSqm" runat="server" Width="200px">20</asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Type</td>
                            <td>
                                <!-- BackColor="#F6F1DB" ForeColor="#7d6754" -->
                                <asp:DropDownList ID="DDLHouseType" runat="server" Width="200px" CssClass="ddl">
                                    <asp:ListItem Selected="True">Apartment</asp:ListItem>
                                    <asp:ListItem>House</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="SearchButton" runat="server" OnClick="SearchButton_Clicked" Text="Evaluate" Width="200" />
                    <br />
                    <br />
                    <asp:TextBox ID="TBResult" runat="server" Height="60px" Style="text-align: center" ReadOnly="True" Width="402px"></asp:TextBox>
                </form>
                <nav>
                    <ul>
                        <li><a href="#" class="icon fa-twitter"><span class="label">Twitter</span></a></li>
                        <li><a href="#" class="icon fa-facebook"><span class="label">Facebook</span></a></li>
                        <!--<li><a href="#" class="icon fa-dribbble"><span class="label">Dribbble</span></a></li>-->
                        <!--<li><a href="#" class="icon fa-github"><span class="label">Github</span></a></li>-->
                        <li><a href="#" class="icon fa-envelope-o"><span class="label">Email</span></a></li>
                    </ul>
                </nav>
            </header>

            <!-- Footer -->
            <footer id="footer">
                <span class="copyright">&copy; Boveta. Design: <a href="http://html5up.net">HTML5 UP</a>.</span>
            </footer>

        </div>
    </div>
</body>
</html>
