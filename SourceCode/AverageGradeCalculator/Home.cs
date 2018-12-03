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

        // Array of grade bands
        public static GradeBand[] gradeBands;

        //The list of courses
        public List<CourseInfor> listCourses;

        // Grade bands object
        public class GradeBand
        {
            public double minPercentage { get; set; }
            public double maxPercentage { get; set; }
            public string letterGrade { get; set; }
            public Color colorGrade { get; set; }
        }

        //Course Information object
        public class CourseInfor
        {
            public double percentGrade { get; set; }
            public string letterGrade { get; set; }
            public Color colorGrade { get; set; }
            public string nameOfCourse { get; set; }

            //Set value for the attributes in the CourseInfor object
            public void SetValue(double percentGrade, string nameOfCourse)
            {
                this.percentGrade = percentGrade;
                this.nameOfCourse = nameOfCourse;
                string letter;
                Color color;

                //Return the letter and color of each band of the grade percentage
                LetterGradeColorCalculator(percentGrade, out letter, out color);
                this.letterGrade = letter;
                this.colorGrade = color;
            }
        }
        public Home()
        {
            InitializeComponent();

            // Settings the first data
            listCourses = new List<CourseInfor>();
            SetupGradeBands();
        }

        // Setup the grade bands
        public void SetupGradeBands()
        {
            gradeBands = new GradeBand[]
            {
                new GradeBand {minPercentage = 80, maxPercentage = 100, letterGrade = "A", colorGrade = Color.Green },
                new GradeBand {minPercentage = 70, maxPercentage = 79, letterGrade = "B", colorGrade = Color.Yellow },
                new GradeBand {minPercentage = 60, maxPercentage = 69, letterGrade = "C", colorGrade = Color.Orange },
                new GradeBand {minPercentage = 50, maxPercentage = 59, letterGrade = "D", colorGrade = Color.OrangeRed },
                new GradeBand {minPercentage = 0, maxPercentage = 49, letterGrade = "F", colorGrade = Color.Red }
            };
        }

        //Calculate the letter and color of grade from percentage of grade
        public static bool LetterGradeColorCalculator(double percentGrade, out string letterGrade, out Color colorGrade)
        {
            letterGrade = "N/A";
            colorGrade = Color.Black;
            for (int i = 0; i < gradeBands.Length; i++)
            {
                if (percentGrade >= gradeBands[i].minPercentage && percentGrade <= gradeBands[i].maxPercentage)
                {
                    //return the letter Grade and Color if percentGrade meets the band
                    letterGrade = gradeBands[i].letterGrade;
                    colorGrade = gradeBands[i].colorGrade;
                    return true;
                }
            }
            return false;
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
                for (int i = 0; i < palViewGrades.Controls.Count / numberOfColumn; i++)
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
                            palViewGrades.Controls[i * numberOfColumn + 3].ForeColor = tempCourse.colorGrade;
                            
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

                CourseInfor finalGrade = new CourseInfor();
                //Calculate the average of all courses
                double averageGrade = Convert.ToInt32(sumGrade / listCourses.Count);

                finalGrade.SetValue(averageGrade, "Final");
                
                //Set value for the Average Label
                lblAverage.Text = finalGrade.percentGrade.ToString();
                
                //Set value for the Letter Average Label
                lblLetterAverage.Text = finalGrade.letterGrade;
                
                //Set the color for the grade
                lblLetterAverage.ForeColor = finalGrade.colorGrade;
                lblAverage.ForeColor = finalGrade.colorGrade;

            }
            catch (Exception)
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
                txtTempOrder.Text = (palViewGrades.Controls.Count / numberOfColumn + 1).ToString();
                txtTempOrder.Location = new Point(pointX, pointY);
                txtTempOrder.Width = 30;
                palViewGrades.Controls.Add(txtTempOrder);

                //Create one Course Name textBox and display
                TextBox txtTempName = new TextBox();
                txtTempName.Name = "txtCourse" + (palViewGrades.Controls.Count / numberOfColumn + 1).ToString();
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
