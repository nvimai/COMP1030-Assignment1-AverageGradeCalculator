using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AverageGradeCalculator
{
    public partial class Home : Form
    {
        //Location of current course name label
        static int pointX = 10;
        static int pointY = 0;
        //Number of Column to show
        static int numberOfColumn = 5;
        //The list of courses
        public List<CourseInfor> listCourses = new List<CourseInfor>();
        //Course Information object
        public class CourseInfor
        {
            public int percentGrade { get; set; }
            public string letterGrade { get; set; }
            public string nameOfCourse { get; set; }
            //Set value for the attributes in the CourseInfor object
            public void SetValue(int percentGrade, string nameOfCourse)
            {
                this.percentGrade = percentGrade;
                this.nameOfCourse = nameOfCourse;
                this.letterGrade = LetterGradeCalculator(percentGrade);
            }
        }
        public Home()
        {
            InitializeComponent();
        }
        //Calculate the letter of grade from percentage of grade
        public static string LetterGradeCalculator(int percentGrade)
        {
            if (percentGrade >= 0 && percentGrade <= 49)
            {
                //return F if percentGrade from 0 to 49
                return "F";
            }
            if (percentGrade >= 50 && percentGrade <= 59)
            {
                //return D if percentGrade from 50 to 59
                return "D";
            }
            if (percentGrade >= 60 && percentGrade <= 69)
            {
                //return C if percentGrade from 60 to 69
                return "C";
            }
            if (percentGrade >= 70 && percentGrade <= 79)
            {
                //return B if percentGrade from 70 to 79
                return "B";
            }
            if (percentGrade >= 80 && percentGrade <= 100)
            {
                //return A if percentGrade from 80 to 100
                return "A";
            }
            //return null for other cases
            return "null";
        }

        //Return the color of each band of the grade percentage
        public static Color ColorOfGrade(int percentGrade)
        {

            if (percentGrade >= 0 && percentGrade <= 49)
            {
                //return Red if percentGrade from 0 to 49
                return Color.Red;
            }
            if (percentGrade >= 50 && percentGrade <= 59)
            {
                //return OrangeRed if percentGrade from 50 to 59
                return Color.OrangeRed;
            }
            if (percentGrade >= 60 && percentGrade <= 69)
            {
                //return Orange if percentGrade from 60 to 69
                return Color.Orange;
            }
            if (percentGrade >= 70 && percentGrade <= 79)
            {
                //return Yellow if percentGrade from 70 to 79
                return Color.Yellow;
            }
            if (percentGrade >= 80 && percentGrade <= 100)
            {
                //return Green if percentGrade from 80 to 100
                return Color.Green;
            }
            //return Black for other cases
            return Color.Black;
        }
        //Numeric checking
        public static bool IsNumber(string stringTemp)
        {
            try
            {
                double temp = Convert.ToDouble(stringTemp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        //Check what if the number is a percentage of not
        public static bool IsPercentage(double numberInput)
        {
            //if numberInput in 0-100 return true
            if (numberInput >= 0 && numberInput <= 100)
                return true;
            return false;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            //Catch an error if it happens
            try
            {
                //Sum of every grade of courses
                double sumGrade = 0;
                //Clear list of courses
                listCourses.Clear();
                //Loop of all courses information
                for (int i = 0; i < palViewGrades.Controls.Count/ numberOfColumn; i++)
                {
                    //Get the name of course from input
                    string tempName = palViewGrades.Controls[i * numberOfColumn + 1].Name;
                    //Numeric checking from the input of grade
                    string tempStringGrade = palViewGrades.Controls[(i * numberOfColumn) + 2].Text;
                    if (IsNumber(tempStringGrade))
                    {
                        //Get the percentage of grade from input (Convert from string to Double then to Int to avoid decimal input error)
                        int tempGrade = Convert.ToInt32(Convert.ToDouble(tempStringGrade));
                        if (IsPercentage(tempGrade))
                        {
                            //Create temperary CourseInfor variable
                            CourseInfor tempCourse = new CourseInfor();
                            //Set the value to the properties of an CourseInfor Object
                            tempCourse.SetValue(tempGrade, tempName);
                            //Set the letter grade for the letter grade label of this course
                            palViewGrades.Controls[i * numberOfColumn + 3].Text = tempCourse.letterGrade;
                            //Set the color for the letter grade label of this course
                            palViewGrades.Controls[i * numberOfColumn + 3].ForeColor = ColorOfGrade(tempCourse.percentGrade);
                            //Add 1 Course into Courses List
                            listCourses.Add(tempCourse);
                            //Plus the grade of this course to sum
                            sumGrade += tempGrade;

                            
                        }
                        else
                        {
                            MessageBox.Show("Not a percentage");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Not a number");
                        return;
                    }
                }
                //Calculate the average of all courses
                int averageGrade = Convert.ToInt32(sumGrade / listCourses.Count);
                //Set value for the Average Label
                lblAverage.Text = averageGrade.ToString();
                //Set value for the Letter Average Label
                string letterGrade = LetterGradeCalculator(averageGrade);
                lblLetterAverage.Text = letterGrade;
                //Set the color for the grade
                Color gradeColor = ColorOfGrade(averageGrade);
                lblLetterAverage.ForeColor = gradeColor;
                lblAverage.ForeColor = gradeColor;

            }
            catch(Exception)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            //Catch an error if it happens
            try
            {

                //Create one order number label and display
                Label txtTempOrder = new Label();
                txtTempOrder.Text = (palViewGrades.Controls.Count/ numberOfColumn + 1).ToString();
                txtTempOrder.Location = new Point(pointX, pointY);
                txtTempOrder.Width = 30;
                palViewGrades.Controls.Add(txtTempOrder);

                //Create one Course Name textBox and display
                TextBox txtTempName = new TextBox();
                txtTempName.Name = "txtCourse" + (palViewGrades.Controls.Count/ numberOfColumn + 1).ToString();
                txtTempName.Location = new Point(pointX + txtTempOrder.Width + 1, pointY);
                txtTempName.Width = 70;
                palViewGrades.Controls.Add(txtTempName);

                //Create one Grade Name textBox and display
                TextBox txtTempGrade = new TextBox();
                txtTempGrade.Name = "txtGrade" + (palViewGrades.Controls.Count / numberOfColumn + 1).ToString();
                txtTempGrade.Location = new Point(pointX + txtTempOrder.Width + txtTempName.Width + 2, pointY);
                txtTempGrade.Width = 50;
                palViewGrades.Controls.Add(txtTempGrade);

                //Create one Letter Grade textBox and display
                Label txtTempLetterGrade = new Label();
                txtTempLetterGrade.Name = "txtLetterGrade" + (palViewGrades.Controls.Count / numberOfColumn + 1).ToString();
                txtTempLetterGrade.Location = new Point(pointX + txtTempOrder.Width + txtTempName.Width + txtTempGrade.Width + 3, pointY);
                txtTempLetterGrade.Width = 20;
                palViewGrades.Controls.Add(txtTempLetterGrade);

                //Create one Course Delete Button and display
                Button btnTempDeleteGrade = new Button();
                btnTempDeleteGrade.Name = "btnDeleteGrade" + (palViewGrades.Controls.Count / numberOfColumn + 1).ToString();
                btnTempDeleteGrade.Text = "X";
                btnTempDeleteGrade.ForeColor = Color.Red;
                btnTempDeleteGrade.Location = new Point(pointX + txtTempOrder.Width + txtTempName.Width + txtTempGrade.Width + txtTempLetterGrade.Width + 4, pointY);
                btnTempDeleteGrade.Click += BtnTempDeleteGrade_Click;
                btnTempDeleteGrade.Width = 20;
                palViewGrades.Controls.Add(btnTempDeleteGrade);

                //Change the location to diploy a new row
                pointY += 21;
            }
            catch (Exception)
            {
                MessageBox.Show(e.ToString());
            }
        }
        //
        private void BtnTempDeleteGrade_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //Delete a row from List of Controls
            DeleteARowFromList(palViewGrades.Controls.IndexOf(btn));
        }

        private bool DeleteARowFromList(int index)
        {
            try
            {
                //check the index is right or not
                if (index >= 0 && index < palViewGrades.Controls.Count)
                {
                    //Remove the delete button
                    palViewGrades.Controls.RemoveAt(index);
                    //Remove the Letter Grade label
                    palViewGrades.Controls.RemoveAt(index - 1);
                    //Remove the Grade textbox
                    palViewGrades.Controls.RemoveAt(index - 2);
                    //Remove the Name of Course textbox
                    palViewGrades.Controls.RemoveAt(index - 3);
                    //Remove the Order label
                    palViewGrades.Controls.RemoveAt(index - 4);
                    

                    //Delete course in List of Courses
                    DeleteCourseInList(index / numberOfColumn);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //Delete a course in the List Of Courses
        private bool DeleteCourseInList(int index)
        {
            try
            {
                //check the index is right or not
                if (index >= 0 && index < listCourses.Count)
                {
                    //Remove the course element in List Of Courses
                    listCourses.RemoveAt(index);
                    listCourses.RemoveAt(index-1);
                    listCourses.RemoveAt(index-2);
                    listCourses.RemoveAt(index-3);
                    listCourses.RemoveAt(index-4);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
