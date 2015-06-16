<?php
    error_reporting(E_ALL);
    ini_set('display_errors', 1);


    $con = mysqli_connect("localhost","root","","unity_test_db");

    // Check connection
    if (mysqli_connect_errno())
    {
        echo "Failed to connect to MySQL: " . mysqli_connect_error();
    }
    
    $fb_id = $con->real_escape_string(isset($_GET['fb_id']) ? $_GET['fb_id'] : '');

    if ( !empty($fb_id) )
    {
        // Perform queries 
        $query = "DELETE FROM `tbl_player_data` WHERE 'fb_id'==$fb_id";
        mysqli_query($con,$query);
    }

    mysqli_close($con);
?>
