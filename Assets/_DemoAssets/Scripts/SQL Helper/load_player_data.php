<?php
    // create the connection to our database with following values: location of our databse
    // (with xampp it's "localhost"), next is the login ("name" and "password").
    // if the connection can not be established we get an error message, that we've entered after "or die"
    error_reporting(E_ALL);
    ini_set('display_errors', 1);

    // after we're logged in, we can call our database
    $con = mysqli_connect("localhost","root","awsasnet","unitytestdb");

    // Check connection
    if (mysqli_connect_errno())
    {
        echo "Failed to connect to MySQL: " . mysqli_connect_error();
    }

    $fb_id = $con->real_escape_string(isset($_GET['fb_id']) ? $_GET['fb_id'] : '');
    
    // now we simply get the scores and sort them by their value. we also add a limit of 5, so we only
    // select the 5 highest values. The * means we search through every value.
    $query = "SELECT * FROM `tbl_player_data` WHERE (`fb_id`='$fb_id' OR  `fb_friends` LIKE CONCAT(  '%',  '$fb_id',  '%' )) ORDER by `score` DESC LIMIT 0 , 10";
    
    // now we store our selected values into a result variable
    $result = mysqli_query($con,$query);
    
    // this will select the whole row we found the score at
    $num_results = mysqli_num_rows($result);  
 
    // at the end we will get 5 rows with only the name and score values in each row
    for($i = 0; $i < $num_results; $i++)
    {
         $row = mysqli_fetch_array($result);
         //echo $row;
         // the echo command is used as the returned value for our Unity Script
         echo $row['fb_id'] . ";" . $row['fb_name'] . ";" . $row['fb_friends'] . ";" . $row['score'] . ";" . $row['jump_data'] . ";" . $row['bonus_data'] . "\n";
    }

    // we're done now, so we can close the connection
    mysqli_close($con);
?>
