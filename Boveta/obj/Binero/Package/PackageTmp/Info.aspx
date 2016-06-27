<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Info.aspx.cs" Inherits="Boveta.Info" %>

<!DOCTYPE html>

<html>
<head>
    <title>Boveta</title>
    <link rel="apple-touch-icon" sizes="57x57" href="/apple-icon-57x57.png">
    <link rel="apple-touch-icon" sizes="60x60" href="/apple-icon-60x60.png">
    <link rel="apple-touch-icon" sizes="72x72" href="/apple-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="76x76" href="/apple-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="114x114" href="/apple-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="120x120" href="/apple-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="144x144" href="/apple-icon-144x144.png">
    <link rel="apple-touch-icon" sizes="152x152" href="/apple-icon-152x152.png">
    <link rel="apple-touch-icon" sizes="180x180" href="/apple-icon-180x180.png">
    <link rel="icon" type="image/png" sizes="192x192"  href="/android-icon-192x192.png">
    <link rel="icon" type="image/png" sizes="32x32" href="/favicon-32x32.png">
    <link rel="icon" type="image/png" sizes="96x96" href="/favicon-96x96.png">
    <link rel="icon" type="image/png" sizes="16x16" href="/favicon-16x16.png">
    <link rel="manifest" href="/manifest.json">
    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="msapplication-TileImage" content="/ms-icon-144x144.png">
    <meta name="theme-color" content="#ffffff">
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="description" content="Automatic house valuator" />
    <meta name="keywords" content="house valuation estimator house pricing" />
    <meta name=viewport content='width=700'>
    <!--[if lte IE 8]><script src="css/ie/html5shiv.js"></script><![endif]-->
    <script src="js/skel.min.js"></script>
    <script src="js/init.js"></script>
    <noscript>
        <link rel="stylesheet" href="css/skel.css" />
        <link rel="stylesheet" href="css/style.css" />
        <link rel="stylesheet" href="css/style-wide.css" />
        <link rel="stylesheet" href="css/style-noscript.css" />
        <link rel="stylesheet" href="css/style-mobile.css" />
        <link rel="stylesheet" href="css/style-mobilep.css" />
    </noscript>
    <!--[if lte IE 9]><link rel="stylesheet" href="css/ie/v9.css" /><![endif]-->
    <!--[if lte IE 8]><link rel="stylesheet" href="css/ie/v8.css" /><![endif]-->
    <style type="text/css">
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
                <p>Automatic House Valuation</p>
                
                <strong>Info</strong>
                <P class="info-paragraph" >
                Boveta is a site to automatically valuate houses and appartments.
                Just enter and address and size before pressing the Evaluate-button.
                The valuation is based on a number of factors including, but not 
                limited to, previously sold homes in the area. Currently accuaracy is approximately +/- 17%.
                </P>
                <strong>
                <br />
                Disclaimer</strong><P class="info-paragraph" >
                Boveta valuations will never be 100% accurate and the results should be regarded as 
                guidelines rather than absolute facts. Boveta takes no responsibility for inacurate valuations.
                </P>
                <nav>
                    <ul>
                        <li><a href="https://twitter.com/BovetaOnline" class="icon fa-twitter"><span class="label">Twitter</span></a></li>
                        <li><a href="https://www.facebook.com/bovetaonline" class="icon fa-facebook"><span class="label">Facebook</span></a></li>
                        <li><a href="Search.aspx" class="icon fa-search"><span class="label">Search</span></a></li>
                        <li><a href="mailto:info@boveta.com" class="icon fa-envelope-o"><span class="label">Email</span></a></li>
                    </ul>
                </nav>
            </header>
        </div>
    </div>
    <!-- Google Analytics -->
    <script>
      (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
      (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
      m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
      })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

      ga('create', 'UA-63674939-1', 'auto');
      ga('send', 'pageview');

    </script>
</body>
</html>
